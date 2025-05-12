using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Utilidad para convertir imágenes entre formatos y flujos de datos.
    /// </summary>
    public static class ImageConverter
    {
        /// <summary>
        /// Convierte una imagen en un arreglo de bytes (por ejemplo para guardar en base de datos o generar PDF).
        /// </summary>
        public static byte[] ToByteArray(Image image, ImageFormat format)
        {
            using var ms = new MemoryStream();
            image.Save(ms, format);
            return ms.ToArray();
        }

        /// <summary>
        /// Carga una imagen desde un arreglo de bytes.
        /// </summary>
        public static Image FromByteArray(byte[] data)
        {
            using var ms = new MemoryStream(data);
            return Image.FromStream(ms);
        }

        /// <summary>
        /// Convierte una imagen a formato PNG.
        /// </summary>
        public static Image ConvertToPng(Image original)
        {
            using var ms = new MemoryStream();
            original.Save(ms, ImageFormat.Png);
            return Image.FromStream(new MemoryStream(ms.ToArray()));
        }
    }
}
