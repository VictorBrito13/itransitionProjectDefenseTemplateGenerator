using System.Security.Cryptography;
using System.Text;

namespace ItransitionTemplates.Utils
{
    public class HashText {
        private static byte[] GetHash(string txt)
        {
            return SHA256.HashData(Encoding.UTF8.GetBytes(txt));
        }

        public static string GetHashString(string txt) {
            StringBuilder sb = new StringBuilder();

            foreach (byte b in GetHash(txt))
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}