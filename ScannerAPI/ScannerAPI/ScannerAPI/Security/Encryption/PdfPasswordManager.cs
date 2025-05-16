// File: Security/Encryption/PdfPasswordManager.cs
using System.IO;
using System.Text;
using iText.Kernel.Pdf;


namespace ScannerAPI.Security.Encryption
{
    /// <summary>
    /// Gestor de contrase�as para archivos PDF usando iText 7.
    /// </summary>
    public class PdfPasswordManager
    {
        /// <summary>
        /// Encripta un PDF aplicando contrase�a de usuario y, opcionalmente, de propietario.
        /// </summary>
        /// <param name="inputPath">Ruta del PDF de entrada.</param>
        /// <param name="outputPath">Ruta donde se guardar� el PDF encriptado.</param>
        /// <param name="userPassword">Contrase�a de usuario para abrir el PDF.</param>
        /// <param name="ownerPassword">Contrase�a de propietario para permisos avanzados (opcional).</param>
        public void EncryptPdf(string inputPath, string outputPath, string userPassword, string? ownerPassword = null)
        {
            // Convierte las contrase�as a bytes
            byte[] userBytes = Encoding.UTF8.GetBytes(userPassword);
            byte[]? ownerBytes = ownerPassword is not null
                ? Encoding.UTF8.GetBytes(ownerPassword)
                : null;

            // Configura la encriptaci�n AES-128 y permisos de impresi�n
            var writerProps = new WriterProperties()
                .SetStandardEncryption(
                    userBytes,
                    ownerBytes,
                    EncryptionConstants.ALLOW_PRINTING,
                    EncryptionConstants.ENCRYPTION_AES_128
                );

            // Abre lector y escritor con las propiedades de encriptaci�n
            using PdfReader pdfReader = new PdfReader(inputPath);
            using PdfWriter pdfWriter = new PdfWriter(outputPath, writerProps);
            using PdfDocument pdfDoc = new PdfDocument(pdfReader, pdfWriter);
            // Al cerrar pdfDoc, el PDF encriptado ya queda escrito.
        }

        /// <summary>
        /// Desencripta un PDF protegido con contrase�a de usuario.
        /// </summary>
        /// <param name="inputPath">Ruta del PDF encriptado.</param>
        /// <param name="outputPath">Ruta donde se guardar� el PDF desencriptado.</param>
        /// <param name="userPassword">Contrase�a de usuario.</param>
        public void DecryptPdf(string inputPath, string outputPath, string userPassword)
        {
            // Prepara las propiedades del lector con contrase�a
            var readerProps = new ReaderProperties()
                .SetPassword(Encoding.UTF8.GetBytes(userPassword));

            using PdfReader pdfReader = new PdfReader(inputPath, readerProps);
            using PdfWriter pdfWriter = new PdfWriter(outputPath);
            using PdfDocument pdfDoc = new PdfDocument(pdfReader, pdfWriter);
            // Al cerrar pdfDoc, el PDF desencriptado ya queda escrito.
        }
    }
}
