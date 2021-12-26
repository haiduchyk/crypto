namespace Crypto
{
    using System;

    internal static class Program
    {
        private static void Main()
        {
            // XorOneByteTask();
            XorByKeyTask();
            // GeneticTask();
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
        
        private static void XorByKeyTask()
        {
            var decrypted = XorByKey.Decrypt();
            Console.WriteLine(decrypted);
        }
    }
}