using ScannerAPI.Models.Events;
using ScannerAPI.Services.Interfaces;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ScannerAPI.Services.Implementations
{
    /// <summary>
    /// Servicio de eventos que almacena y publica eventos escaneados en memoria.
    /// </summary>
    public class EventBusService : IEventBusService
    {
        private readonly ConcurrentDictionary<string, ScanEvent> _eventStore = new();

        /// <summary>
        /// Publica un nuevo evento para una sesión de escaneo.
        /// </summary>
        /// <param name="sessionId">ID de sesión de escaneo.</param>
        /// <param name="scanEvent">Evento a registrar.</param>
        public Task PublishAsync(string sessionId, ScanEvent scanEvent)
        {
            _eventStore[sessionId] = scanEvent;
            return Task.CompletedTask;
        }

        /// <summary>
        /// Obtiene el último evento registrado para una sesión.
        /// </summary>
        /// <param name="sessionId">ID de sesión de escaneo.</param>
        /// <returns>Evento si existe, null si no.</returns>
        public Task<ScanEvent?> GetLastEventAsync(string sessionId)
        {
            _eventStore.TryGetValue(sessionId, out var evt);
            return Task.FromResult(evt);
        }
    }
}
