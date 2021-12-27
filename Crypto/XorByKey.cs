namespace Crypto
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class XorByKey
    {
        private static readonly byte[] text;

        static XorByKey()
        {
            var base64 = File.ReadAllText(@".\..\..\..\task2.txt");
            text = Convert.FromBase64String(base64);
        }

        public static string Decrypt()
        {
            var keyLength = KeyLengthCalculator.GetKeyLength(text);
            var sections = GetSectionsByKeyLength(keyLength, text);
            var decryptedSections = sections.Select(section => XorOneByte.Decrypt(section.ToArray())).ToList();
            return JoinSelections(decryptedSections, keyLength);
        }

        private static string JoinSelections(IReadOnlyList<string> selections, int keyLength)
        {
            var result = new StringBuilder();
            for (var i = 0; i < selections.Min(str => str.Length); i++)
            {
                for (var j = 0; j < keyLength; j++)
                {
                    result.Append(selections[j][i]);
                }
            }

            var lastLetterIndex = selections[0].Length - 1;

            for (var j = 0; j < keyLength; j++)
            {
                if (selections[j].Length == lastLetterIndex + 1)
                {
                    result.Append(selections[j][lastLetterIndex]);
                }
            }

            return result.ToString();
        }

        private static IEnumerable<List<byte>> GetSectionsByKeyLength(int keyLength, IReadOnlyList<byte> text)
        {
            return text
                .Select((t, i) => new {Index = i, Value = t})
                .GroupBy(x => x.Index % keyLength)
                .Select(x => x.Select(v => v.Value).ToList());
        }
    }
}