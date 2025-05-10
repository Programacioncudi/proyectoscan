namespace ScannerAPI.Interfaces
{
    public interface IImageProcessor
    {
        byte[] ProcessImage(byte[] imageData);
        string ConvertToFormat(string imagePath, string format);
    }
}