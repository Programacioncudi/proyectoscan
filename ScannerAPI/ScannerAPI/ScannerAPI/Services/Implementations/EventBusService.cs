using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Services.Implementations
{
    public class EventBusService : IEventBusService
    {
        private readonly ConcurrentDictionary<Type, List<Func<object, Task>>> _handlers = new();
        public Task PublishAsync<TEvent>(TEvent evt)
        {
            if (_handlers.TryGetValue(typeof(TEvent), out var list))
                return Task.WhenAll(list.ConvertAll(h => h(evt)));
            return Task.CompletedTask;
        }
        public void Subscribe<TEvent>(Func<TEvent, Task> handler)
        {
            var list = _handlers.GetOrAdd(typeof(TEvent), _ => new List<Func<object, Task>>());
            list.Add(e => handler((TEvent)e));
        }
    }
}
