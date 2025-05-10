namespace ScannerAPI.Models.Events
{
    public class ScanProgress
    {
        public int Percentage { get; set; }
        public string StatusMessage { get; set; }
        public string ScanId { get; set; }
    }
}