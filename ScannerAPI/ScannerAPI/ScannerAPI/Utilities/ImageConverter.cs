using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Convierte y redimensiona imágenes en memoria.
    /// </summary>
    public static class ImageConverter
    {
        public static byte[] ConvertFormat(byte[] data, ImageFormat format)
        {
            using var msIn = new MemoryStream(data);
            using var img = Image.FromStream(msIn);
            using var msOut = new MemoryStream();
            img.Save(msOut, format);
            return msOut.ToArray();
        }

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
