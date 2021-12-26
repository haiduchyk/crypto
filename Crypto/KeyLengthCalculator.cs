namespace Crypto
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class KeyLengthCalculator
    {
        private const double EnglishCoincidenceIndex = 0.0625;
        private const int MaxKeyLenght = 8;

        public static int GetKeyLength(IReadOnlyList<byte> text)
        {
            var sections = GetPossibleSections(text);
            var coincidences = sections
                .Select(s => Math.Abs(GetCoincidenceIndex(s) - EnglishCoincidenceIndex)).ToList();
            return coincidences.IndexOf(coincidences.Min()) + 1;
        }

        private static IEnumerable<List<byte>> GetPossibleSections(IReadOnlyList<byte> text)
        {
            var sections = new List<List<byte>>();
            for (var i = 1; i <= MaxKeyLenght; i++)
            {
                var section = new List<byte>();
                for (var j = 0; j < text.Count; j += i)
                {
                    section.Add(text[j]);
                }

                sections.Add(section);
            }

            return sections;
        }

        private static double GetCoincidenceIndex(IReadOnlyCollection<byte> text)
        {
            double sum = 0;
            for (var i = 0; i < 256; i++)
            {
                var letterCount = text.Count(x => x == i);
                sum += letterCount * letterCount;
            }

            return sum / (text.Count * text.Count);
        }
    }
}