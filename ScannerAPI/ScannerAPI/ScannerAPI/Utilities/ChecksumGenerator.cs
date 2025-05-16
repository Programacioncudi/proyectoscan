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
        /// <summary>
        /// Calcula el checksum SHA-256 de una cadena de texto.
        /// </summary>
        /// <param name="input">Cadena de entrada a hashear.</param>
        /// <returns>Checksum SHA-256 de la cadena en formato hexadecimal.</returns>
        public static string ComputeSha256(string input)
        {
            // Usar el método estático SHA256.HashData en lugar de ComputeHash
            var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(hashBytes);
        }

        /// <summary>
        /// Calcula el checksum SHA-256 de un archivo.
        /// </summary>
        /// <param name="filePath">Ruta completa del archivo.</param>
        /// <returns>Checksum SHA-256 del contenido del archivo en formato hexadecimal.</returns>
        public static string ComputeFileSha256(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            // Método estático HashData para secuencias
            var hashBytes = SHA256.HashData(stream);
            return Convert.ToHexString(hashBytes);
        }
    }
}
