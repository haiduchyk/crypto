namespace Lab4
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class PasswordsGenerator
    {
        public static List<PasswordModificator> Modificators;
        public static Random Random = new();
        private static readonly float TotalWeight;
        private const int MaxModificationsAmount = 2;

        private static string[] allPasswords;
        private static string[] topPasswords;


        private const float RandomPercent = 0.03f;
        private const float TopHundredPercent = 0.1f;
        private const float HumanLikePercent = 0.1f;

        private const string PossibleCharactersForRandomPassword =
            "abcdefghijklmnopqrstuvwxyz123456789!\"#$%&\'()*+,-./:;<=>?@[\\]^_`{|}~";

        private const int RandomMaxSize = 16;
        private const int RandomMinSize = 8;

        static PasswordsGenerator()
        {
            InitializePasswordModificators();
            ReadPasswords();
            TotalWeight = Modificators.Sum(m => m.Weight);
        }

        private static void InitializePasswordModificators()
        {
            Modificators = new List<PasswordModificator>
            {
                new AddNumbersInEndModificator(),
                new AddCharacterModificator(),
                new ReplaceCharacterModificator(),
                new ReverseModificator(),
                new LowerCaseModificator(),
                new UpperCaseModificator(),
            };
        }

        private static void ReadPasswords()
        {
            allPasswords = File.ReadAllLines("./../../../100k_passwords.txt");
            topPasswords = File.ReadAllLines("./../../../top100_common_passwords.txt");
        }

        public static string[] GeneratePasswords(int amount)
        {
            var passwords = new string[amount];
            for (var i = 0; i < amount; i++)
            {
                var next = Random.NextDouble();
                if (next < RandomPercent)
                {
                    passwords[i] = GenerateRandomPassword();
                }
                else if (next < RandomPercent + TopHundredPercent)
                {
                    passwords[i] = GetTop100Password();
                }
                else if (next < RandomPercent + TopHundredPercent + HumanLikePercent)
                {
                    passwords[i] = GetHumanLikePassword();
                }
                else
                {
                    passwords[i] = GetWeakPassword();
                }
            }

            return passwords;
        }

        private static string GenerateRandomPassword()
        {
            var builder = new StringBuilder();
            var lenght = Random.Next(RandomMinSize, RandomMaxSize + 1);
            for (var i = 0; i < lenght; i++)
            {
                var index = Random.Next(PossibleCharactersForRandomPassword.Length);
                builder.Append(PossibleCharactersForRandomPassword[index]);
            }

            return builder.ToString();
        }

        private static string GetTop100Password()
        {
            return topPasswords[Random.Next(topPasswords.Length)];
        }
        
        private static string GetWeakPassword()
        {
            return topPasswords[Random.Next(topPasswords.Length)];
        }

        public static string GetHumanLikePassword()
        {
            var startString = allPasswords[Random.Next(allPasswords.Length)];
            var passwordBuilder = new StringBuilder(startString);
            ApplyRandomModificatiosnTo(passwordBuilder);
            var result = passwordBuilder.ToString();
            return result;
        }

        private static void ApplyRandomModificatiosnTo(StringBuilder passwordBuilder)
        {
            var modificationsAmount = Random.Next(MaxModificationsAmount) + 1;
            for (var i = 0; i < modificationsAmount; i++)
            {
                var m = SelectRandomModificator();
                m.Modify(passwordBuilder);
            }
        }

        private static PasswordModificator? SelectRandomModificator()
        {
            var rand = Random.NextDouble() * TotalWeight;
            var max = TotalWeight;
            return Modificators.FirstOrDefault(x => rand >= (max -= x.Weight));
        }
    }
}