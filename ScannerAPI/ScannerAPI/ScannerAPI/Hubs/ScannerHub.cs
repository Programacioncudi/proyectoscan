using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ScannerAPI.Hubs
{
    /// <summary>
    /// Hub de SignalR para comunicar eventos de escaneo en tiempo real.
    /// </summary>
    public class ScannerHub : Hub
    {
        /// <summary>
        /// Método llamado cuando un cliente se une a un grupo por ID de escáner.
        /// </summary>
        public async Task JoinScannerGroup(string scannerId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, scannerId);
        }

        /// <summary>
        /// Método llamado cuando un cliente sale del grupo.
        /// </summary>
        public async Task LeaveScannerGroup(string scannerId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, scannerId);
        }
    }
}
