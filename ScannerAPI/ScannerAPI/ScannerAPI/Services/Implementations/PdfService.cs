using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Canvas.Parser.Listener;

namespace ScannerAPI.Services.Implementations
{
    public class PdfService
    {
        public async Task<string> ExtractTextAsync(string path)
        {
            using var reader = new PdfReader(path);
            using var pdf = new PdfDocument(reader);
            var sb = new System.Text.StringBuilder();
            for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
            {
                var strategy = new SimpleTextExtractionStrategy();
                sb.AppendLine(PdfTextExtractor.GetTextFromPage(pdf.GetPage(i), strategy));
            }
            return await Task.FromResult(sb.ToString());
        }
    }
}
