using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Genera checksums SHA-256 para cadenas y archivos.
    /// </summary>
    public static class ChecksumGenerator
    {
        public static string ComputeSha256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes);
        }

        public static string ComputeFileSha256(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(stream);
            return Convert.ToHexString(bytes);
        }
    }
}
