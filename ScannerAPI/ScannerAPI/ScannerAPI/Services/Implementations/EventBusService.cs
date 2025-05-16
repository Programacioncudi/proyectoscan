// File: Services/Implementations/EventBusService.cs
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Services.Implementations
{
    /// <summary>
    /// Servicio de bus de eventos en memoria que permite publicar y suscribirse a eventos de diferentes tipos.
    /// </summary>
    public class EventBusService : IEventBusService
    {
        private readonly ConcurrentDictionary<Type, List<Func<object, Task>>> _handlers = new();

        /// <summary>
        /// Publica un evento a todos los suscriptores registrados para su tipo.
        /// </summary>
        /// <typeparam name="TEvent">Tipo del evento a publicar.</typeparam>
        /// <param name="evt">Instancia del evento que se va a publicar.</param>
        /// <returns>Una tarea que representa la operación de publicación asincrónica.</returns>
        public Task PublishAsync<TEvent>(TEvent evt)
        {
            if (_handlers.TryGetValue(typeof(TEvent), out var list))
                return Task.WhenAll(list.ConvertAll(h => h(evt!)));
            return Task.CompletedTask;
        }

        /// <summary>
        /// Registra un manejador para eventos de tipo <typeparamref name="TEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">Tipo de evento al que se suscribe.</typeparam>
        /// <param name="handler">Función asincrónica que será llamada cuando se publique un evento de ese tipo.</param>
        public void Subscribe<TEvent>(Func<TEvent, Task> handler)
        {
            var list = _handlers.GetOrAdd(typeof(TEvent), _ => new List<Func<object, Task>>());
            list.Add(e => handler((TEvent)e!));
        }
    }
}
