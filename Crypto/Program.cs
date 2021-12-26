namespace Crypto
{
    using System;

    internal static class Program
    {
        private static void Main()
        {
            XorOneByteTask();
            GeneticTask();
        }

        private static void GeneticTask()
        {
            var decrypted = Genetic.Decrypt();
            Console.WriteLine(decrypted);
        }

        private static void XorOneByteTask()
        {
            var decrypted = XorOneByte.DecryptTask();
            Console.WriteLine(decrypted);
        }
    }
}