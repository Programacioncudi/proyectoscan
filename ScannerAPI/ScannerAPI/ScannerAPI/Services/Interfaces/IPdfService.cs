using System.Drawing;

namespace ScannerAPI.Services.Interfaces
{
    /// <summary>
    /// Interfaz para servicio que convierte imágenes en documentos PDF.
    /// </summary>
    public interface IPdfService
    {
        /// <summary>
        /// Genera un archivo PDF a partir de una imagen.
        /// </summary>
        /// <param name="image">Imagen escaneada procesada</param>
        /// <param name="outputPath">Ruta donde guardar el PDF</param>
        /// <returns>Ruta del archivo PDF generado</returns>
        string GenerateFromImage(Image image, string outputPath);
    }
}
