using System.Security.Cryptography;
using System.Text;

namespace ItransitionTemplates.Utils
{
    public class HashText {
        private const int Iterations = 100000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        public static string GetHashString(string txt) {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(txt),
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize
            );

            byte[] combined = new byte[salt.Length + hash.Length];
            Buffer.BlockCopy(salt, 0, combined, 0, salt.Length);
            Buffer.BlockCopy(hash, 0, combined, salt.Length, hash.Length);

            return Convert.ToBase64String(combined);
        }

        public static bool VerifyHash(string txt, string storedHash) {
            byte[] combined;
            try {
                combined = Convert.FromBase64String(storedHash);
            } catch (FormatException) {
                return false;
            }
            if (combined.Length < SaltSize + HashSize) return false;

            byte[] salt = new byte[SaltSize];
            Buffer.BlockCopy(combined, 0, salt, 0, SaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(txt),
                salt,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize
            );

            byte[] storedHashPart = new byte[HashSize];
            Buffer.BlockCopy(combined, SaltSize, storedHashPart, 0, HashSize);

            return CryptographicOperations.FixedTimeEquals(hash, storedHashPart);
        }

        public static bool VerifyOldSha256Hash(string txt, string storedHash) {
            if (storedHash == null || storedHash.Length != 64) return false;

            foreach (char c in storedHash) {
                if (!Uri.IsHexDigit(c)) return false;
            }

            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(txt));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash) sb.Append(b.ToString("x2"));

            return sb.ToString() == storedHash.ToLower();
        }
    }
}