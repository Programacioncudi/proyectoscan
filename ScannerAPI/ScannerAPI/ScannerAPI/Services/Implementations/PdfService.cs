// File: Services/Implementations/PdfService.cs
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf;
using System.Threading.Tasks;
using System.Text;
namespace ScannerAPI.Services.Implementations
{
    /// <summary>
    /// Servicio para extraer texto de documentos PDF usando iText 7.
    /// </summary>
    public class PdfService
    {
        /// <summary>
        /// Extrae todo el texto de un PDF dado su ruta.
        /// </summary>
        /// <param name="path">Ruta al archivo PDF.</param>
        /// <returns>Una cadena con el texto extraído de todas las páginas.</returns>
        public async Task<string> ExtractTextAsync(string path)
        {
            // iText7: PdfReader y PdfDocument se encuentran en iText.Kernel.Pdf
            using var reader = new PdfReader(path);
            using var pdf = new PdfDocument(reader);
            var sb = new System.Text.StringBuilder();

            for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
            {
                var strategy = new SimpleTextExtractionStrategy();
                sb.AppendLine(
                    PdfTextExtractor.GetTextFromPage(
                        pdf.GetPage(i),
                        strategy
                    )
                );
            }

            // Se retorna el resultado como tarea completada
            return await Task.FromResult(sb.ToString());
        }
    }
}
