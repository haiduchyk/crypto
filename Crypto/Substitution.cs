namespace Crypto
{
    using System.Linq;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class Substitution
    {
        private static readonly char[] ciphertext;
        private static readonly Dictionary<string, double> threeGramIndexes = new();
        private static readonly Random random = new();

        private const double ExpectedIndex = 0.00097;
        private const int AmountOfBestFromPopulation = 300;
        private const int ChanceForMutationFromZeroToHundred = 60;
        private const int MutationsAmount = 2;
        private static readonly char[] arrayForDecryption;

        static Substitution()
        {
            ReadThreeGrams();
            ciphertext = File.ReadAllText(@".\..\..\..\task3.txt").ToCharArray();
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

        private static double EstimateBasedOnThreeGrams(char[] populationItem)
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
            var population = GetFirstPopulation(AmountOfBestFromPopulation);
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
                    $"\ngeneration: {generation}; best: {new string(best)}; estimation: {bestEstimation * 1000}");
                Console.WriteLine(DecryptSubstitution(ciphertext, best));
            } while (bestEstimation < ExpectedIndex);


            var decrypted = DecryptSubstitution(ciphertext, GetBest(population, 1)[0]);
            return decrypted;
        }

        private static string DecryptSubstitution(char[] cipherText, char[] key)
        {
            for (var i = 0; i < cipherText.Length; i++)
            {
                var c = cipherText[i];
                arrayForDecryption[i] = Constants.AllLetters[Array.IndexOf(key, c)];
            }

            return new string(arrayForDecryption);
        }

        private static char[] GetFirstPopulationItem()
        {
            var result = new char[Constants.AllLetters.Length];
            var letters = Constants.AllLetters.ToList();

            for (var i = 0; i < Constants.AllLetters.Length; i++)
            {
                var letter = letters[random.Next(0, letters.Count)];
                result[i] = letter;
                letters.Remove(letter);
            }

            return result;
        }

        private static List<char[]> GetFirstPopulation(int populationLength)
        {
            var population = new List<char[]>();
            for (var i = 0; i < populationLength; i++)
            {
                var item = GetFirstPopulationItem();
                population.Add(item);
            }

            return population;
        }

        private static List<char[]> GetBest(List<char[]> population, int aliveCount)
        {
            return population.Distinct().OrderByDescending(EstimateBasedOnThreeGrams).Take(aliveCount).ToList();
        }

        private static void Crossing(List<char[]> bestFromPopulation)
        {
            var children = new List<char[]>();
            for (var i = 1; i < bestFromPopulation.Count * 2; i++)
            {
                var index1 = random.Next(bestFromPopulation.Count);
                var index2 = random.Next(bestFromPopulation.Count);
                children.Add(Cross(bestFromPopulation[index1], bestFromPopulation[index2]));
            }

            bestFromPopulation.AddRange(children);
        }

        private static void MutatePopulation(List<char[]> population)
        {
            var newChildren =
                (from c in population
                    let rnd = random.Next(100)
                    where rnd <= ChanceForMutationFromZeroToHundred
                    select Mutate(c)).ToList();
            population.AddRange(newChildren);
        }

        public static char[] Mutate(char[] item)
        {
            var newItem = (char[]) item.Clone();
            for (var i = 0; i < random.Next(MutationsAmount) + 1; i++)
            {
                var index1 = random.Next(item.Length);
                var index2 = random.Next(item.Length);
                (newItem[index1], newItem[index2]) = (newItem[index2], newItem[index1]);
            }

            return newItem;
        }

        public static char[] Cross(char[] firstParent, char[] secondParent)
        {
            var child = new char[firstParent.Length];
            var letters = Constants.AllLetters.ToList();

            for (var i = 0; i < firstParent.Length; i++)
            {
                var firstParentChar = firstParent[i];
                var secondParentChar = secondParent[i];

                var isContainFromFirstParent = child.Contains(firstParentChar);
                var isContainFromSecondParent = child.Contains(secondParentChar);

                if (isContainFromFirstParent && isContainFromSecondParent)
                {
                    var randomLetter = letters[random.Next(0, letters.Count)];
                    SetLetter(randomLetter);
                }
                else if (isContainFromFirstParent)
                {
                    SetLetter(secondParentChar);
                }
                else if (isContainFromSecondParent)
                {
                    SetLetter(firstParentChar);
                }
                else if (firstParentChar == secondParentChar)
                {
                    SetLetter(firstParentChar);
                }
                else
                {
                    SetLetter(random.Next(2) == 0 ? firstParentChar : secondParentChar);
                }

                void SetLetter(char letter)
                {
                    child[i] = letter;
                    letters.Remove(letter);
                }
            }

            return child;
        }
    }
}