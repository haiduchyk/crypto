namespace Crypto
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public static class EnglishTextAnalyzer
    {
        private static readonly double[] Count = new double[Constants.AllLetters.Length];
        private static int wrongCharactersAmount;
        private static string text;

        private static readonly List<double> EnglishFrequencies = new()
        {
            0.08167, 0.01492, 0.02782, 0.04253, 0.12702, 0.02228, 
            0.02015, 0.06094, 0.06966, 0.00153, 0.00772, 0.04025, 
            0.02406, 0.06749, 0.07507, 0.01929, 0.00095, 0.05987, 
            0.06327, 0.09056, 0.02758, 0.00978, 0.02360, 0.00150, 
            0.01974, 0.00074
        };

        private const string AllowedCharacters = "[.,_()“”\\s-]";

        public static double CalculateChiSquared(string text)
        {
            LowerAndRemovePunctuation(text);
            FillCount();
            return GetChi();
        }

        private static void LowerAndRemovePunctuation(string value)
        {
            text = value.ToLower();
            text = Regex.Replace(value, AllowedCharacters, "");
        }

        private static void FillCount()
        {
            ClearCount();

            foreach (var c in text)
            {
                if (c >= 97 && c <= 122)
                {
                    Count[c - 97]++;
                }
                else
                {
                    wrongCharactersAmount++;
                }
            }
        }

        private static void ClearCount()
        {
            wrongCharactersAmount = 0;
            for (var i = 0; i < Count.Length; i++)
            {
                Count[i] = 0;
            }
        }

        private static double GetChi()
        {
            double result = 0;

            for (var i = 0; i < EnglishFrequencies.Count; i++)
            {
                var observed = Count[i];
                var expected = text.Length * EnglishFrequencies[i];
                var difference = observed - expected;
                result += difference * difference / expected;
            }

            return result + wrongCharactersAmount * wrongCharactersAmount;
        }
    }
}