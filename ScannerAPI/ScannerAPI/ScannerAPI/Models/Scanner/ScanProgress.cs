namespace ScannerAPI.Models.Scanner;

public class ScanProgress
{
    public int Percentage { get; set; }
    public string Message { get; set; }

    public ScanProgress(int percentage, string message)
    {
        Percentage = percentage;
        Message = message;
    }
}