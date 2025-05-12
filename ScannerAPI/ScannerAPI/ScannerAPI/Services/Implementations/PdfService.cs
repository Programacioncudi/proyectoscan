using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using ScannerAPI.Services.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ScannerAPI.Services.Implementations
{
    /// <summary>
    /// Servicio que convierte imágenes escaneadas en un único archivo PDF.
    /// </summary>
    public class PdfService : IPdfService
    {
        /// <summary>
        /// Genera un archivo PDF a partir de una lista de imágenes.
        /// </summary>
        /// <param name="images">Lista de imágenes en formato byte array</param>
        /// <returns>PDF generado como array de bytes</returns>
        public async Task<byte[]> GeneratePdfFromImagesAsync(List<byte[]> images)
        {
            using var document = new PdfDocument();

            foreach (var imageBytes in images)
            {
                using var ms = new MemoryStream(imageBytes);
                using var image = XImage.FromStream(() => ms);

                var page = document.AddPage();
                page.Width = image.PixelWidth;
                page.Height = image.PixelHeight;

                using var gfx = XGraphics.FromPdfPage(page);
                gfx.DrawImage(image, 0, 0, page.Width, page.Height);
            }

            using var output = new MemoryStream();
            document.Save(output, false);
            return await Task.FromResult(output.ToArray());
        }
    }
}
