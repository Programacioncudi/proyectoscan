using ScannerAPI.Services.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ScannerAPI.Utilities;

/// <summary>
/// Procesador de imágenes que permite formatear y comprimir imágenes escaneadas.
/// </summary>
public class ImageProcessor : IImageProcessor
{
    /// <summary>
    /// Procesa una imagen a partir de datos binarios, aplicando formato y calidad.
    /// </summary>
    /// <param name="imageData">Bytes de la imagen original</param>
    /// <param name="format">Formato deseado (JPEG, PNG, etc.)</param>
    /// <param name="quality">Calidad de compresión (0-100)</param>
    /// <returns>Objeto con datos procesados de imagen</returns>
    public Task<ProcessedImage> ProcessImageAsync(byte[] imageData, string format, int quality)
    {
        using var ms = new MemoryStream(imageData);
        using var image = Image.FromStream(ms);
        
        using var outputMs = new MemoryStream();
        var imageFormat = GetImageFormat(format);
        var codec = GetImageCodecInfo(imageFormat);

        if (codec == null)
        {
            throw new InvalidDataException($"Codec no disponible para formato: {format}");
        }

        var encoderParams = new EncoderParameters(1);
        encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

        image.Save(outputMs, codec, encoderParams);

        return Task.FromResult(new ProcessedImage
        {
            Data = outputMs.ToArray(),
            Format = format,
            Width = image.Width,
            Height = image.Height
        });
    }

    private ImageFormat GetImageFormat(string format)
    {
        return format.ToUpper() switch
        {
            "PNG" => ImageFormat.Png,
            "GIF" => ImageFormat.Gif,
            "BMP" => ImageFormat.Bmp,
            "TIFF" => ImageFormat.Tiff,
            _ => ImageFormat.Jpeg
        };
    }

    private ImageCodecInfo? GetImageCodecInfo(ImageFormat format)
    {
        return ImageCodecInfo.GetImageEncoders()
            .FirstOrDefault(codec => codec.FormatID == format.Guid);
    }
}
