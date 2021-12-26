namespace Crypto
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class XorOneByte
    {
        private static byte[] text;
        
        static XorOneByte()
        {
            var text16 = File.ReadAllText(@".\..\..\..\task1.txt");

            text = Enumerable.Range(0, text16.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(text16.Substring(x, 2), 16))
                .ToArray();
        }

        public static string DecryptTask()
        {
            return Decrypt(text);
        }
        
        public static string Decrypt(byte[] text)
        {
            var results = new string[256];
            var result = new byte[text.Length];

            for (var i = 0; i < 256; i++)
            {
                for (var j = 0; j < result.Length; j++)
                {
                    result[j] = (byte) (text[j] ^ i);
                }

                results[i] = Encoding.UTF8.GetString(result);
            }

            var best = results.OrderBy(EnglishTextAnalyzer.CalculateChiSquared).First();
            return best;
        }
    }
}