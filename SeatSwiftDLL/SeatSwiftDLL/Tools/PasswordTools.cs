using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SeatSwiftDLL.Tools
{
    public class PasswordTools
    {
        public static byte[] HashPassword(string password, byte[] salt)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("The password cannot be null or empty");

            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] passwordWithSalt = new byte[passwordBytes.Length + salt.Length];
                Array.Copy(passwordBytes, 0, passwordWithSalt, 0, passwordBytes.Length);
                Array.Copy(salt, 0, passwordWithSalt, passwordBytes.Length, salt.Length);

                return sha256.ComputeHash(passwordWithSalt);
            }
        }

        public static byte[] CreateSalt()
        {
            byte[] salt = new byte[8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public static bool VerifyPassword(string password, byte[] hash, byte[] salt)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("The password cannot be null or empty");
            if (hash == null || hash.Length == 0)
                throw new ArgumentNullException("The hash cannot be null or empty");

            byte[] hashedPassword = HashPassword(password, salt);

            // Simple byte array comparison
            for (int i = 0; i < hashedPassword.Length; i++)
            {
                if (hashedPassword[i] != hash[i])
                    return false;
            }
            return true;
        }
    }
}
