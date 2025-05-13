using System.Text;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Encryption;

namespace ScannerAPI.Security.Encryption
{
    public class PdfPasswordManager
    {
        public void EncryptPdf(string inputPath, string outputPath, string userPassword)
        {
            var writer = new PdfWriter(outputPath, new WriterProperties()
                .SetStandardEncryption(Encoding.UTF8.GetBytes(userPassword), null,
                    EncryptionConstants.ALLOW_PRINTING, EncryptionConstants.ENCRYPTION_AES_128));
            var pdf = new PdfDocument(new PdfReader(inputPath), writer);
            pdf.Close();
        }

        public void DecryptPdf(string inputPath, string outputPath, string userPassword)
        {
            var reader = new PdfReader(inputPath, new ReaderProperties().SetPassword(Encoding.UTF8.GetBytes(userPassword)));
            var writer = new PdfWriter(outputPath);
            var pdf = new PdfDocument(reader, writer);
            pdf.Close();
        }
    }
}