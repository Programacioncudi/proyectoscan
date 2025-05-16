// File: Security/Encryption/PdfPasswordManager.cs
using System.IO;
using System.Text;
using iText.Kernel.Pdf;


namespace ScannerAPI.Security.Encryption
{
    /// <summary>
    /// Gestor de contraseñas para archivos PDF usando iText 7.
    /// </summary>
    public class PdfPasswordManager
    {
        /// <summary>
        /// Encripta un PDF aplicando contraseña de usuario y, opcionalmente, de propietario.
        /// </summary>
        /// <param name="inputPath">Ruta del PDF de entrada.</param>
        /// <param name="outputPath">Ruta donde se guardará el PDF encriptado.</param>
        /// <param name="userPassword">Contraseña de usuario para abrir el PDF.</param>
        /// <param name="ownerPassword">Contraseña de propietario para permisos avanzados (opcional).</param>
        public void EncryptPdf(string inputPath, string outputPath, string userPassword, string? ownerPassword = null)
        {
            // Convierte las contraseñas a bytes
            byte[] userBytes = Encoding.UTF8.GetBytes(userPassword);
            byte[]? ownerBytes = ownerPassword is not null
                ? Encoding.UTF8.GetBytes(ownerPassword)
                : null;

            // Configura la encriptación AES-128 y permisos de impresión
            var writerProps = new WriterProperties()
                .SetStandardEncryption(
                    userBytes,
                    ownerBytes,
                    EncryptionConstants.ALLOW_PRINTING,
                    EncryptionConstants.ENCRYPTION_AES_128
                );

            // Abre lector y escritor con las propiedades de encriptación
            using PdfReader pdfReader = new PdfReader(inputPath);
            using PdfWriter pdfWriter = new PdfWriter(outputPath, writerProps);
            using PdfDocument pdfDoc = new PdfDocument(pdfReader, pdfWriter);
            // Al cerrar pdfDoc, el PDF encriptado ya queda escrito.
        }

        /// <summary>
        /// Desencripta un PDF protegido con contraseña de usuario.
        /// </summary>
        /// <param name="inputPath">Ruta del PDF encriptado.</param>
        /// <param name="outputPath">Ruta donde se guardará el PDF desencriptado.</param>
        /// <param name="userPassword">Contraseña de usuario.</param>
        public void DecryptPdf(string inputPath, string outputPath, string userPassword)
        {
            // Prepara las propiedades del lector con contraseña
            var readerProps = new ReaderProperties()
                .SetPassword(Encoding.UTF8.GetBytes(userPassword));

            using PdfReader pdfReader = new PdfReader(inputPath, readerProps);
            using PdfWriter pdfWriter = new PdfWriter(outputPath);
            using PdfDocument pdfDoc = new PdfDocument(pdfReader, pdfWriter);
            // Al cerrar pdfDoc, el PDF desencriptado ya queda escrito.
        }
    }
}
