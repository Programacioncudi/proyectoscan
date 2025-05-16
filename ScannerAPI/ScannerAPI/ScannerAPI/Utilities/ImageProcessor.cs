using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Processing;
using System.IO;
using System.Threading.Tasks;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Utilities
{
    /// <summary>
    /// Implementación de <see cref="IImageProcessor"/> usando ImageSharp para operaciones de imagen multiplataforma
    /// </summary>
    public class ImageProcessor : IImageProcessor
    {
        /// <summary>
        /// Convierte los datos de imagen al formato especificado
        /// </summary>
        /// <param name="imageData">Bytes de la imagen de entrada</param>
        /// <param name="format">Formato de salida deseado</param>
        /// <returns>Bytes de la imagen convertida</returns>
        public async Task<byte[]> ConvertToFormatAsync(byte[] imageData, FileFormat format)
        {
            return await Task.Run(() =>
            {
                using var image = Image.Load(imageData);
                using var outputStream = new MemoryStream();

                IImageEncoder encoder = format switch
                {
                    FileFormat.Jpeg => new JpegEncoder(),
                    FileFormat.Bmp => new BmpEncoder(),
                    FileFormat.Tiff => new TiffEncoder(),
                    FileFormat.Pdf => new PngEncoder(),  // Conversión intermedia para PDF
                    _ => new PngEncoder(),
                };

                image.Save(outputStream, encoder);
                return outputStream.ToArray();
            });
        }

        /// <summary>
        /// Cambia el tamaño de la imagen a las dimensiones especificadas
        /// </summary>
        /// <param name="imageData">Bytes de la imagen de entrada</param>
        /// <param name="width">Ancho deseado en píxeles</param>
        /// <param name="height">Alto deseado en píxeles</param>
        /// <returns>Bytes de la imagen redimensionada</returns>
        public async Task<byte[]> ResizeAsync(byte[] imageData, int width, int height)
        {
            return await Task.Run(() =>
            {
                using var image = Image.Load(imageData);
                using var outputStream = new MemoryStream();

                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(width, height),
                    Mode = ResizeMode.Stretch  // Ajusta exactamente a las dimensiones
                }));

                image.Save(outputStream, new PngEncoder());
                return outputStream.ToArray();
            });
        }
    }
}