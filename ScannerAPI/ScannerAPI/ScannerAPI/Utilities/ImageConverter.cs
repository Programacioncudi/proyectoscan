// File: Utilities/ImageConverter.cs
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Versioning;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Convierte y redimensiona imágenes en memoria.
    /// Solo soportado en plataformas Windows.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static class ImageConverter
    {
        /// <summary>
        /// Convierte el array de bytes de una imagen a un formato especificado.
        /// </summary>
        /// <param name="data">Bytes de la imagen original.</param>
        /// <param name="format">Formato de imagen de salida.</param>
        /// <returns>Array de bytes de la imagen convertida.</returns>
        public static byte[] ConvertFormat(byte[] data, ImageFormat format)
        {
            using var msIn = new MemoryStream(data);
            using var img = Image.FromStream(msIn);
            using var msOut = new MemoryStream();
            img.Save(msOut, format);
            return msOut.ToArray();
        }

        /// <summary>
        /// Redimensiona el array de bytes de una imagen al ancho y alto especificados.
        /// </summary>
        /// <param name="data">Bytes de la imagen original.</param>
        /// <param name="width">Ancho deseado del thumbnail.</param>
        /// <param name="height">Alto deseado del thumbnail.</param>
        /// <returns>Array de bytes de la imagen redimensionada.</returns>
        public static byte[] Resize(byte[] data, int width, int height)
        {
            using var msIn = new MemoryStream(data);
            using var img = Image.FromStream(msIn);
            using var thumb = new Bitmap(img, width, height);
            using var msOut = new MemoryStream();
            thumb.Save(msOut, img.RawFormat);
            return msOut.ToArray();
        }
    }
}

