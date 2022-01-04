using System;

namespace Common.Helpers
{
    public static class GenerateHashSha
    {
        public static string HashSha512(this string hashPassword)
        {
            var salt = Constants.AuthConfig.SaltConfig;
            string saltAndPwd = String.Concat(hashPassword, salt);

            var bytes = System.Text.Encoding.UTF8.GetBytes(saltAndPwd);

            using var hash = System.Security.Cryptography.SHA512.Create();

            var hashedInputBytes = hash.ComputeHash(bytes);

            var hashedInputStringBuilder = new System.Text.StringBuilder(128);

            foreach (var b in hashedInputBytes)
            {
                hashedInputStringBuilder.Append(b.ToString("X2"));
            }
            return hashedInputStringBuilder.ToString();
        }

        public static bool CheckHash(this string password, string hashPassword)
        {
            var hash = HashSha512(password);
            if (hash.Equals(hashPassword))
            {
                return true;
            }
            return false;
        }
    }
}
