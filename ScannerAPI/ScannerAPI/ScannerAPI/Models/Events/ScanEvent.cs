namespace ScannerAPI.Models.Events;

public class ScanEvent
{
    public string SessionId { get; set; }
    public string Message { get; set; }
    public int Progress { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}