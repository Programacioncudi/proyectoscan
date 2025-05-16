// File: Services/Interfaces/IEventBusService.cs
using System;
using System.Threading.Tasks;

namespace ScannerAPI.Services.Interfaces
{
    /// <summary>
    /// Define un bus de eventos en memoria que permite publicar y suscribirse a eventos genéricos.
    /// </summary>
    public interface IEventBusService
    {
        /// <summary>
        /// Publica un evento de tipo <typeparamref name="TEvent"/> a todos los suscriptores registrados.
        /// </summary>
        /// <typeparam name="TEvent">Tipo del evento a publicar.</typeparam>
        /// <param name="evt">Instancia del evento a publicar.</param>
        Task PublishAsync<TEvent>(TEvent evt);

        /// <summary>
        /// Registra un manejador para eventos de tipo <typeparamref name="TEvent"/>.
        /// </summary>
        /// <typeparam name="TEvent">Tipo de evento al que se suscribe.</typeparam>
        /// <param name="handler">Función asincrónica que manejará el evento cuando se publique.</param>
        void Subscribe<TEvent>(Func<TEvent, Task> handler);
    }
}
