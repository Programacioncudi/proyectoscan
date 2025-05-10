namespace ScannerAPI.Models.Scanner;

public class DeviceCapabilities
{
    public bool SupportsDuplex { get; set; }
    public int[] SupportedResolutions { get; set; }
    public string[] SupportedFormats { get; set; }
}