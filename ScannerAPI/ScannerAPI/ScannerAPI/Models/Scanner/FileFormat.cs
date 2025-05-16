// File: Models/Scanner/FileFormat.cs
namespace ScannerAPI.Models.Scanner
{
    /// <summary>
    /// Formatos de archivo soportados para la imagen escaneada.
    /// </summary>
    public enum FileFormat
    {
        /// <summary>Imagen PNG.</summary>
        Png,

        /// <summary>Imagen JPEG.</summary>
        Jpeg,

        /// <summary>Imagen BMP.</summary>
        Bmp,

        /// <summary>Imagen TIFF.</summary>
        Tiff,

        /// <summary>Documento PDF.</summary>
        Pdf
    }
}
