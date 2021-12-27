namespace Lab2
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    internal static class Program
    {
        private static byte[] line1;
        private static byte[] line2;
        private static List<byte[]> allLines;
        private static byte[] xoredLines;

        private const int FirstLineIndex = 4;
        private const int SecondLineIndex = 5;

        private static void Main()
        {
            ReadLines();

            SetupLines();
            while (true)
            {
                // var part = "That patient";
                var part = GetLinePartFromUser();
                TryXorWithPart(part);
            }
            
            // For who would bear the ships and scorns of time,
            // Th'oppressor's wrong, tle proud man's contumely,
            // The pangs of dispriz'd hove, the law's delay,
            // The insolence of office( and the spurns
            // That patient merit of tl'unworthy takes,
            // When he himself might hms quietus make
            // With a bare bodkin? Who$would fardels bear,
            // To grunt and sweat undev a weary life,
            // But that the dread of skmething after death,
            // The undiscovere'd countvy, from whose bourn
            // No traveller returns, pqzzles the will,
            // And makes us rather beav those ills we have
            // Than fly to others that$we know not of?
            // Thus conscience doth maoe cowards of us all,
            // And thus the native hue$of resolution
            // Is sicklied o'er with tle pale cast of thought,
            // And enterprises of greap pith and moment
            // With this regard their gurrents turn awry
            // And lose the name of acpion.
        }

        private static void ReadLines()
        {
            string[] lines = File.ReadAllLines(@".\..\..\..\text.txt");
            allLines = lines.Select(ToByteArrayFrom16).ToList();
        }

        static byte[] ToByteArrayFrom16(string text)
        {
            return Enumerable.Range(0, text.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(text.Substring(x, 2), 16))
                .ToArray();
        }

        private static void SetupLines()
        {
            line1 = allLines[FirstLineIndex];
            line2 = allLines[SecondLineIndex];
            xoredLines = Xor(line1, line2);
        }

        private static string? GetLinePartFromUser()
        {
            return Console.ReadLine();
        }

        private static byte[] Xor(IReadOnlyList<byte> t1, IReadOnlyList<byte> t2)
        {
            var minLength = Math.Min(t1.Count, t2.Count);
            byte[] result = new byte[minLength];
            for (var i = 0; i < minLength; i++)
            {
                result[i] = (byte) (t1[i] ^ t2[i]);
            }

            return result;
        }

        private static void TryXorWithPart(string part)
        {
            Console.WriteLine($"part => {part}");
            var bytesPart = Encoding.ASCII.GetBytes(part);
            for (var i = 0; i < xoredLines.Length - part.Length; i++)
            {
                var samePartInXoredLines = xoredLines.Skip(i).Take(bytesPart.Length).ToArray();
                var bytesResult = Xor(bytesPart, samePartInXoredLines);
                var result = Encoding.ASCII.GetString(bytesResult);
                Console.WriteLine($"{i} : {result}");
            }
        }
    }
}