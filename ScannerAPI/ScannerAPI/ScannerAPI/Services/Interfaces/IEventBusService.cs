using System;
using System.Threading.Tasks;

namespace ScannerAPI.Services.Interfaces
{
    public interface IEventBusService
    {
        Task PublishAsync<TEvent>(TEvent evt);
        void Subscribe<TEvent>(Func<TEvent, Task> handler);
    }
}