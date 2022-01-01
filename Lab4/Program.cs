namespace Lab4
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    internal static class Program
    {
        private static SHA1 sha1;
        private static RandomNumberGenerator random;
        private static MD5 md5;
        private const int BcryptWorkFactor = 4;
        private const int PasswordsAmount = 100_000;

        private static void Main()
        {
            sha1 = SHA1.Create();
            random = RandomNumberGenerator.Create();
            md5 = MD5.Create();

            var passwords = PasswordsGenerator.GeneratePasswords(PasswordsAmount);
            File.WriteAllLines("../../../md5_hash.csv", passwords.Select(GenerateMd5Hash));
            File.WriteAllLines("../../../sha1_hash_and_salt.csv", passwords.Select(GenerateSha1HashAndSaltString));
            File.WriteAllLines("../../../bcrypt_hash.csv", passwords.Select(GenerateBCryptHash));
        }

        private static string GenerateMd5Hash(string password)
        {
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        private static string GenerateSha1HashAndSaltString(string password)
        {
            var salt = new byte[16];
            random.GetBytes(salt);
            var stringSalt = Convert.ToBase64String(salt);
            var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password + stringSalt));
            return Convert.ToBase64String(hash) + ";" + stringSalt;
        }

        private static string GenerateBCryptHash(string password)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt(BcryptWorkFactor);
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }
    }
}