using ScannerAPI.Models.Events;
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Services.Implementations;

public class EventBusService : IEventBusService
{
    public Task PublishScanEventAsync(string sessionId, string message, int progress)
    {
        // Implementación real usaría SignalR u otro bus de eventos
        return Task.CompletedTask;
    }

    public Task PublishProgressEvent(string sessionId, ScanProgress progress)
    {
        // Implementación real
        return Task.CompletedTask;
    }

    public Task<ScanEvent> GetLastEventAsync(string sessionId)
    {
        // Implementación real
        return Task.FromResult<ScanEvent>(null);
    }
}