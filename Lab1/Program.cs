namespace Lab1
{
    using System;

    internal static class Program
    {
        private static void Main()
        {
            // XorOneByteTask();
            // XorByKeyTask();
            // SubstitutionTask();
            PolySubstitutionTask();
   
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
        
        private static void SubstitutionTask()
        {
            var decrypted = Substitution.Decrypt();
            Console.WriteLine(decrypted);
        }
        
        private static void PolySubstitutionTask()
        {
            var decrypted = PolySubstitution.Decrypt();
            Console.WriteLine(decrypted);
        }
    }
}