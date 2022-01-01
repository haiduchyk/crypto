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
        }

        // public static string GeneratePassword()
        // {
        //     return null;
        // }

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