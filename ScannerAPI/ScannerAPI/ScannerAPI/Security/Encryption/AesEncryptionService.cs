// File: Security/Encryption/AesEncryptionService.cs
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ScannerAPI.Security.Encryption
{
    /// <summary>
    /// Servicio para encriptar y desencriptar texto usando AES.
    /// </summary>
    public class AesEncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        /// <summary>
        /// Crea una nueva instancia de <see cref="AesEncryptionService"/>.
        /// </summary>
        /// <param name="base64Key">Clave AES en formato Base64.</param>
        /// <param name="base64Iv">Vector de inicialización (IV) en formato Base64.</param>
        public AesEncryptionService(string base64Key, string base64Iv)
        {
            _key = Convert.FromBase64String(base64Key);
            _iv = Convert.FromBase64String(base64Iv);
        }

        /// <summary>
        /// Encripta un texto plano y retorna el resultado en Base64.
        /// </summary>
        /// <param name="plainText">Texto plano a encriptar.</param>
        /// <returns>Texto cifrado en Base64.</returns>
        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            using var encryptor = aes.CreateEncryptor();
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs, Encoding.UTF8))
            {
                sw.Write(plainText);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <summary>
        /// Desencripta un texto cifrado en Base64 y retorna el texto plano.
        /// </summary>
        /// <param name="cipherText">Texto cifrado en Base64.</param>
        /// <returns>Texto original desencriptado.</returns>
        public string Decrypt(string cipherText)
        {
            var buffer = Convert.FromBase64String(cipherText);
            using var aes = Aes.Create();
            aes.Key = _key;
            aes.IV = _iv;
            using var decryptor = aes.CreateDecryptor();
            using var ms = new MemoryStream(buffer);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs, Encoding.UTF8);
            return sr.ReadToEnd();
        }
    }
}
