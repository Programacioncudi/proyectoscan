using System.Security.Cryptography;
using System.Text;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Genera hash de archivos o datos en memoria.
    /// </summary>
    public static class ChecksumGenerator
    {
        /// <summary>
        /// Calcula el hash SHA256 de una cadena.
        /// </summary>
        public static string ComputeSHA256(string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }

        /// <summary>
        /// Calcula el hash MD5 de un archivo.
        /// </summary>
        public static string ComputeMD5(Stream stream)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(stream);
            return Convert.ToHexString(hash);
        }
    }
}
