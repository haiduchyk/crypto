namespace Lab4
{
    using System;

    internal static class Program
    {
        private static void Main()
        {
            var passwords = PasswordsGenerator.GeneratePasswords(1000);
        }
    }
}