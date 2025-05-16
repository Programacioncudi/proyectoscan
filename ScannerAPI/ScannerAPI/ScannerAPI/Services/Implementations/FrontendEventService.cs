// File: Services/Implementations/FrontendEventService.cs
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ScannerAPI.Hubs;
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Services.Implementations
{
    /// <summary>
    /// Servicio de eventos que publica directamente en el frontend a través de SignalR.
    /// </summary>
    public class FrontendEventService : IEventBusService
    {
        private readonly IHubContext<ScannerHub> _hub;

        /// <summary>
        /// Crea una nueva instancia de <see cref="FrontendEventService"/>.
        /// </summary>
        /// <param name="hub">Contexto de SignalR para enviar eventos.</param>
        public FrontendEventService(IHubContext<ScannerHub> hub)
            => _hub = hub;

        /// <summary>
        /// Publica un evento de tipo <typeparamref name="TEvent"/> a todos los clientes conectados.
        /// </summary>
        /// <typeparam name="TEvent">Tipo del evento que se va a enviar.</typeparam>
        /// <param name="evt">Instancia del evento a enviar.</param>
        /// <returns>Una tarea que representa la operación asincrónica de envío.</returns>
        public Task PublishAsync<TEvent>(TEvent evt)
            => _hub.Clients.All.SendAsync(typeof(TEvent).Name, evt);

        /// <summary>
        /// Suscribe un manejador a eventos de tipo <typeparamref name="TEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">Tipo de evento al que se suscribe.</typeparam>
        /// <param name="handler">Función que maneja el evento.</param>
        /// <remarks>
        /// En este servicio no se aplica la suscripción de manejadores, ya que publica directamente al frontend.
        /// </remarks>
        public void Subscribe<TEvent>(Func<TEvent, Task> handler)
        {
            // No aplica en la publicación directa a SignalR.
        }
    }
}

