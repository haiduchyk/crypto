namespace Lab1
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static class PolySubstitution
    {
        private static readonly char[] ciphertext;
        private static readonly Dictionary<string, double> threeGramIndexes = new();
        private static readonly Random random = new();

        private const double ExpectedIndex = 0.00107;
        private const int AmountOfBestFromPopulation = 2000;
        private static readonly char[] arrayForDecryption;
        private const int KeyLength = 4;
        private const int MutationIterationsAmount = 3;

        static PolySubstitution()
        {
            ReadThreeGrams();
            ciphertext = File.ReadAllText(@".\..\..\..\task4.txt").ToCharArray();
            arrayForDecryption = new char[ciphertext.Length];
        }

        private static void ReadThreeGrams()
        {
            var threeGramsCounts = new Dictionary<string, decimal>();
            decimal sum = 0;
            var text = File.ReadAllLines(@".\..\..\..\3grams.txt");

            foreach (var line in text)
            {
                var key = line[..3];
                var sub = line.Substring(4, line.Length - 4);
                var value = decimal.Parse(sub);
                threeGramsCounts.Add(key.ToUpper(), value);
                sum += value;
            }

            foreach (var (key, value) in threeGramsCounts)
            {
                var count = decimal.ToDouble(decimal.Divide(value, sum));
                threeGramIndexes.Add(key, count);
            }
        }

        private static double EstimateBasedOnThreeGrams(char[][] populationItem)
        {
            var decrypted = DecryptSubstitution(ciphertext, populationItem);
            return GetThreeGramsValue(decrypted);
        }

        private static double GetThreeGramsValue(string text)
        {
            double value = 0;
            for (var i = 0; i < text.Length - 2; i++)
            {
                var sub = text.Substring(i, 3);
                value += threeGramIndexes[sub];
            }

            return value / (text.Length - 2);
        }

        public static string Decrypt()
        {
            var generation = 0;
            var population = GetFirstPopulation();
            // population.Add(new char[][]
            // {
            //     "TEUIJYSPBCAOHFKNDLMZRVQWXG".ToCharArray(),
            //     "YQJBEUNIDPFLATMRXHOKZCWSVG".ToCharArray(),
            //     "OQFPASKZHCLTJUIXVDYRGEWBMN".ToCharArray(),
            //     "LJGONRPKCBIHDTUEFSQYXAZVMW".ToCharArray()
            //
            // });

            double bestEstimation;
            do
            {
                population = GetBest(population, AmountOfBestFromPopulation);
                Crossing(population);
                MutatePopulation(population);

                var best = GetBest(population, 1)[0];
                bestEstimation = EstimateBasedOnThreeGrams(best);
                generation++;

                Console.WriteLine(
                    $"\ngeneration: {generation}; best: {string.Join(' ', best.Select(c => new string(c)))} estimation: {bestEstimation * 1000}");
                Console.WriteLine(DecryptSubstitution(ciphertext, best));
            } while (bestEstimation < ExpectedIndex);


            var decrypted = DecryptSubstitution(ciphertext, GetBest(population, 1)[0]);
            return decrypted;
        }

        private static string DecryptSubstitution(char[] cipherText, char[][] key)
        {
            for (var i = 0; i < cipherText.Length; i++)
            {
                var c = cipherText[i];
                var k = key[i % KeyLength];
                var index = Array.IndexOf(k, c);
                arrayForDecryption[i] = Constants.AllLetters[index];
            }

            return new string(arrayForDecryption);
        }

        private static char[][] GetFirstPopulationItem()
        {
            var key = new char[KeyLength][];

            for (var j = 0; j < KeyLength; j++)
            {
                var keyPart = new char[Constants.AllLetters.Length];
                var letters = Constants.AllLetters.ToList();

                for (var i = 0; i < Constants.AllLetters.Length; i++)
                {
                    var letter = letters[random.Next(0, letters.Count)];
                    keyPart[i] = letter;
                    letters.Remove(letter);
                }

                key[j] = keyPart;
            }

            return key;
        }

        private static List<char[][]> GetFirstPopulation()
        {
            var population = new List<char[][]>();

            for (var i = 0; i < AmountOfBestFromPopulation; i++)
            {
                var item = GetFirstPopulationItem();
                population.Add(item);
            }

            return population;
        }

        private static List<char[][]> GetBest(List<char[][]> population, int aliveCount)
        {
            return population.Distinct().OrderByDescending(EstimateBasedOnThreeGrams).Take(aliveCount).ToList();
        }

        private static void Crossing(List<char[][]> bestFromPopulation)
        {
            var children = new List<char[][]>();
            for (var i = 1; i < bestFromPopulation.Count * 2; i++)
            {
                var index1 = random.Next(bestFromPopulation.Count);
                var index2 = random.Next(bestFromPopulation.Count);
                children.Add(Cross(bestFromPopulation[index1], bestFromPopulation[index2]));
            }

            bestFromPopulation.AddRange(children);
        }

        private static void MutatePopulation(List<char[][]> population)
        {
            var newChildren = new List<char[][]>();
            foreach (var c in population)
            {
                for (var i = 0; i < MutationIterationsAmount; i++)
                {
                    newChildren.Add(Mutate(c));
                }
            }

            population.AddRange(newChildren);
        }

        private static char[][] Mutate(char[][] item)
        {
            var newItem = new char[KeyLength][];
            for (var i = 0; i < KeyLength; i++)
            {
                newItem[i] = Substitution.Mutate(item[i]);
            }

            return newItem;
        }

        private static char[][] Cross(char[][] firstParent, char[][] secondParent)
        {
            var child = new char[KeyLength][];
            for (var i = 0; i < firstParent.Length; i++)
            {
                child[i] = Substitution.Cross(firstParent[i], secondParent[i]);
            }

            return child;
        }
    }
}