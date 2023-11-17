using System.Security.Cryptography;
using System.Text;

namespace SkiService.Helpers
{
    public static class HashingHelper
    {
        public static string ConvertToSha256(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                // Konvertiere den Input-String in ein Byte-Array und berechne den Hash
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Konvertiere das Byte-Array zurück in einen String
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
