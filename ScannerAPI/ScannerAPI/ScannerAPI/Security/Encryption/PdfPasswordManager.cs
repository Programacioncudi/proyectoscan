using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.Security;
using System.IO;

namespace ScannerAPI.Security.Encryption
{
    /// <summary>
    /// Servicio para encriptar archivos PDF con contraseña.
    /// </summary>
    public class PdfPasswordManager
    {
        /// <summary>
        /// Aplica contraseña a un PDF existente y devuelve el resultado como byte array.
        /// </summary>
        public byte[] ApplyPassword(byte[] pdfBytes, string password)
        {
            using var input = new MemoryStream(pdfBytes);
            var document = PdfReader.Open(input, PdfDocumentOpenMode.Modify);

            var security = document.SecuritySettings;
            security.UserPassword = password;
            security.OwnerPassword = password;
            security.PermitPrint = true;
            security.PermitExtractContent = false;
            security.PermitModifyDocument = false;

            using var output = new MemoryStream();
            document.Save(output, false);
            return output.ToArray();
        }
    }
}
