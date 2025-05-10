using ScannerAPI.Services.Interfaces;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace ScannerAPI.Utilities;

public class ImageProcessor : IImageProcessor
{
    public Task<ProcessedImage> ProcessImageAsync(byte[] imageData, string format, int quality)
    {
        using var ms = new MemoryStream(imageData);
        using var image = Image.FromStream(ms);
        
        using var outputMs = new MemoryStream();
        var imageFormat = GetImageFormat(format);
        
        var encoderParams = new EncoderParameters(1);
        encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
        
        image.Save(outputMs, GetImageCodecInfo(imageFormat), encoderParams);
        
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

    private ImageCodecInfo GetImageCodecInfo(ImageFormat format)
    {
        var codecs = ImageCodecInfo.GetImageEncoders();
        foreach (var codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return null;
    }
}