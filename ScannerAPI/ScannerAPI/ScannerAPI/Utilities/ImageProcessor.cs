using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Procesa imágenes: convierte formatos y ajusta dimensión.
    /// </summary>
    public class ImageProcessor : IImageProcessor
    {
        public Task<byte[]> ConvertToFormatAsync(byte[] imageData, FileFormat format)
        {
            using var msIn = new MemoryStream(imageData);
            using var img = Image.FromStream(msIn);
            using var msOut = new MemoryStream();
            img.Save(msOut, format switch
            {
                FileFormat.JPEG => ImageFormat.Jpeg,
                FileFormat.PNG => ImageFormat.Png,
                FileFormat.BMP => ImageFormat.Bmp,
                _ => ImageFormat.Png
            });
            return Task.FromResult(msOut.ToArray());
        }

        public Task<byte[]> ResizeAsync(byte[] imageData, int width, int height)
        {
            using var msIn = new MemoryStream(imageData);
            using var img = Image.FromStream(msIn);
            using var thumb = new Bitmap(img, width, height);
            using var msOut = new MemoryStream();
            thumb.Save(msOut, img.RawFormat);
            return Task.FromResult(msOut.ToArray());
        }
    }
}
