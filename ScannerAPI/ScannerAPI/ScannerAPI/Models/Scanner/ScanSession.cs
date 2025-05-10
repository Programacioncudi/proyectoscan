namespace ScannerAPI.Models.Scanner;

public class ScanSession
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public DateTime StartTime { get; set; }
    public string DeviceId { get; set; }
    public string Status { get; set; }
}