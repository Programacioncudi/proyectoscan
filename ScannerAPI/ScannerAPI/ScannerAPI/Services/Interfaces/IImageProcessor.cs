using System.Threading.Tasks;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Services.Interfaces
{
    public interface IImageProcessor
    {
        Task<byte[]> ConvertToFormatAsync(byte[] imageData, FileFormat format);
        Task<byte[]> ResizeAsync(byte[] imageData, int width, int height);
    }
}