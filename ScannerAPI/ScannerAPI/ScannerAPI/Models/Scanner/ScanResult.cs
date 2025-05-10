namespace ScannerAPI.Models.Scanner;

public class ScanResult
{
    public byte[] ImageData { get; set; }
    public string Format { get; set; }
    public ScanMetadata Metadata { get; set; }
}

public class ScanMetadata
{
    public int Width { get; set; }
    public int Height { get; set; }
    public int DPI { get; set; }
    public int SizeKB { get; set; }
}