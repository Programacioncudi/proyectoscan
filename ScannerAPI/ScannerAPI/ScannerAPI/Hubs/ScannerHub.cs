using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace ScannerAPI.Hubs
{
    /// <summary>
    /// Hub de SignalR para notificaciones de escaneo.
    /// </summary>
    [Authorize]
    public class ScannerHub : Hub
    {
        /// <summary>
        /// Une la conexión actual al grupo identificado por <paramref name="scannerId"/>.
        /// </summary>
        /// <param name="scannerId">ID del escáner cuyo grupo se va a unir.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task JoinScannerGroup(string scannerId)
        {
            if (string.IsNullOrWhiteSpace(scannerId))
                throw new HubException("scannerId inválido.");

            await Groups.AddToGroupAsync(Context.ConnectionId, scannerId);
        }

        /// <summary>
        /// Sale del grupo de escaneo identificado por <paramref name="scannerId"/>.
        /// </summary>
        /// <param name="scannerId">ID del escáner cuyo grupo se va a abandonar.</param>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        public async Task LeaveScannerGroup(string scannerId)
        {
            if (string.IsNullOrWhiteSpace(scannerId))
                throw new HubException("scannerId inválido.");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, scannerId);
        }

        /// <summary>
        /// Notifica a todos los clientes del grupo <paramref name="scannerId"/> que el escaneo ha comenzado.
        /// </summary>
        /// <param name="scannerId">ID del escáner.</param>
        /// <param name="scanId">ID del escaneo iniciado.</param>
        /// <returns>Una tarea que representa la operación de notificación.</returns>
        public Task NotifyScanStarted(string scannerId, string scanId)
            => Clients.Group(scannerId).SendAsync("ScanStarted", scanId);

        /// <summary>
        /// Notifica el progreso del escaneo al grupo <paramref name="scannerId"/>.
        /// </summary>
        /// <param name="scannerId">ID del escáner.</param>
        /// <param name="scanId">ID del escaneo en curso.</param>
        /// <param name="percent">Porcentaje completado del escaneo.</param>
        /// <returns>Una tarea que representa la operación de notificación.</returns>
        public Task NotifyScanProgress(string scannerId, string scanId, int percent)
            => Clients.Group(scannerId).SendAsync("ScanProgress", scanId, percent);

        /// <summary>
        /// Notifica la finalización del escaneo al grupo <paramref name="scannerId"/> con la URL del archivo.
        /// </summary>
        /// <param name="scannerId">ID del escáner.</param>
        /// <param name="scanId">ID del escaneo completado.</param>
        /// <param name="fileUrl">URL donde se puede descargar el archivo escaneado.</param>
        /// <returns>Una tarea que representa la operación de notificación.</returns>
        public Task NotifyScanCompleted(string scannerId, string scanId, string fileUrl)
            => Clients.Group(scannerId).SendAsync("ScanCompleted", scanId, fileUrl);

        /// <summary>
        /// Notifica un error ocurrido durante el escaneo al grupo <paramref name="scannerId"/>.
        /// </summary>
        /// <param name="scannerId">ID del escáner.</param>
        /// <param name="scanId">ID del escaneo que falló.</param>
        /// <param name="errorMessage">Mensaje de error descrito.</param>
        /// <returns>Una tarea que representa la operación de notificación.</returns>
        public Task NotifyScanError(string scannerId, string scanId, string errorMessage)
            => Clients.Group(scannerId).SendAsync("ScanError", scanId, errorMessage);
    }
}
