namespace ScannerAPI.Interfaces
{
    public interface ITwainConfig
    {
        string ScannerName { get; set; }
        int DPI { get; set; }
        string OutputPath { get; set; }
    }
}