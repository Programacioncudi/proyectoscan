using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ScannerAPI.Hubs;
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Services.Implementations
{
    public class FrontendEventService : IEventBusService
    {
        private readonly IHubContext<ScannerHub> _hub;
        public FrontendEventService(IHubContext<ScannerHub> hub) => _hub = hub;
        public Task PublishAsync<TEvent>(TEvent evt)
            => _hub.Clients.All.SendAsync(typeof(TEvent).Name, evt);
        public void Subscribe<TEvent>(Func<TEvent, Task> handler) { /* no aplica */ }
    }
}
