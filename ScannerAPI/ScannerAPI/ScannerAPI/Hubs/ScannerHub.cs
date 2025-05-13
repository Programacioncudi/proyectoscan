// File: Hubs/ScannerHub.cs
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
        public async Task JoinScannerGroup(string scannerId)
        {
            if (string.IsNullOrWhiteSpace(scannerId))
                throw new HubException("scannerId inválido.");

            await Groups.AddToGroupAsync(Context.ConnectionId, scannerId);
        }

        public async Task LeaveScannerGroup(string scannerId)
        {
            if (string.IsNullOrWhiteSpace(scannerId))
                throw new HubException("scannerId inválido.");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, scannerId);
        }

        public Task NotifyScanStarted(string scannerId, string scanId)
            => Clients.Group(scannerId).SendAsync("ScanStarted", scanId);

        public Task NotifyScanProgress(string scannerId, string scanId, int percent)
            => Clients.Group(scannerId).SendAsync("ScanProgress", scanId, percent);

        public Task NotifyScanCompleted(string scannerId, string scanId, string fileUrl)
            => Clients.Group(scannerId).SendAsync("ScanCompleted", scanId, fileUrl);

        public Task NotifyScanError(string scannerId, string scanId, string errorMessage)
            => Clients.Group(scannerId).SendAsync("ScanError", scanId, errorMessage);
    }
}
