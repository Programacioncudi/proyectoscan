using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using ScannerAPI.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ScannerAPI.Services
{
    /// <summary>
    /// Servicio PDF para generación de documentos a partir de imágenes.
    /// </summary>
    public class PdfService : IPdfService
    {
        /// <summary>
        /// Crea un PDF con múltiples imágenes.
        /// </summary>
        public async Task<byte[]> GeneratePdfFromImagesAsync(List<byte[]> images)
        {
            using var doc = new PdfDocument();

            foreach (var bytes in images)
            {
                using var ms = new MemoryStream(bytes);
                using var img = XImage.FromStream(() => ms);

                var page = doc.AddPage();
                page.Width = img.PixelWidth;
                page.Height = img.PixelHeight;

                using var gfx = XGraphics.FromPdfPage(page);
                gfx.DrawImage(img, 0, 0);
            }

            using var output = new MemoryStream();
            doc.Save(output, false);
            return await Task.FromResult(output.ToArray());
        }
    }
}
