using Microsoft.AspNetCore.SignalR;
using ScannerAPI.Hubs;
using System.Threading.Tasks;

namespace ScannerAPI.Services
{
    /// <summary>
    /// Servicio para enviar eventos de escaneo al frontend vía SignalR.
    /// </summary>
    public class FrontendEventService
    {
        private readonly IHubContext<ScannerHub> _hubContext;

        public FrontendEventService(IHubContext<ScannerHub> hubContext)
        {
            _hubContext = hubContext;
        }

        /// <summary>
        /// Envía un mensaje de progreso del escaneo al cliente.
        /// </summary>
        public async Task SendProgressAsync(string scannerId, int percentage, string? status = null)
        {
            await _hubContext.Clients.Group(scannerId).SendAsync("ScanProgress", new
            {
                Percentage = percentage,
                Status = status ?? "En progreso"
            });
        }

        /// <summary>
        /// Envía un mensaje indicando que el escaneo fue completado.
        /// </summary>
        public async Task SendScanCompletedAsync(string scannerId, string filePath)
        {
            await _hubContext.Clients.Group(scannerId).SendAsync("ScanCompleted", new
            {
                FilePath = filePath
            });
        }

        /// <summary>
        /// Envía un mensaje de error al cliente.
        /// </summary>
        public async Task SendErrorAsync(string scannerId, string errorMessage)
        {
            await _hubContext.Clients.Group(scannerId).SendAsync("ScanError", new
            {
                Message = errorMessage
            });
        }
    }
}
