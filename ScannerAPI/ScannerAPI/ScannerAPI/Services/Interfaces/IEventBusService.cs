using ScannerAPI.Models.Events;

namespace ScannerAPI.Services.Interfaces
{
    public interface IEventBusService
    {
        void PublishScanProgress(ScanProgress progress);
        void SubscribeToScanEvents(Action<ScanProgress> handler);
    }
}