using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ScannerAPI.Database;
using ScannerAPI.Exceptions;
using ScannerAPI.Hubs;
using ScannerAPI.Infrastructure.Storage;
using ScannerAPI.Infrastructure.Wrappers;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Services.Implementations
{
    /// <inheritdoc/>
    /// <summary>
    /// Crea una instancia de <see cref="ScannerService"/>.
    /// </summary>
    /// <param name="wrappers">Colección de wrappers de escáner disponibles.</param>
    /// <param name="storage">Servicio de almacenamiento de archivos.</param>
    /// <param name="context">Contexto de base de datos.</param>
    /// <param name="hubContext">Contexto de SignalR para notificaciones.</param>
    public class ScannerService(
        IEnumerable<IScannerWrapper> wrappers,
        IStorageService storage,
        ApplicationDbContext context,
        IHubContext<ScannerHub> hubContext) : IScannerService
    {
        private readonly IEnumerable<IScannerWrapper> _wrappers = wrappers ?? throw new ArgumentNullException(nameof(wrappers));
        private readonly IStorageService _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        private readonly ApplicationDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly IHubContext<ScannerHub> _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));

        /// <inheritdoc/>
        /// <remarks>
        /// Lanza <see cref="DomainException"/> si no hay un wrapper compatible.
        /// </remarks>
        public async Task<ScanResult> ScanAsync(ScanOptions options, CancellationToken cancellationToken)
        {
            var wrapper = _wrappers.FirstOrDefault(w => w.Supports(options))
                          ?? throw new DomainException("NoScanner", "No se encontró un escáner compatible.");

            var fileName = $"{Guid.NewGuid()}.{options.Format}".ToLowerInvariant();
            var tempPath = Path.Combine(Path.GetTempPath(), fileName);

            var result = await wrapper.ScanAsync(options, tempPath, cancellationToken);
            var bytes = await File.ReadAllBytesAsync(tempPath, cancellationToken);
            var finalPath = await _storage.SaveFileAsync(fileName, bytes);

            var record = new ScanRecord
            {
                Id = Guid.NewGuid(),
                ScanId = result.ScanId,
                FilePath = finalPath,
                Success = result.Success,
                ErrorMessage = result.ErrorMessage,
                SessionId = Guid.Empty
            };

            _context.ScanRecords.Add(record);
            await _context.SaveChangesAsync(cancellationToken);

            await _hubContext.Clients
                .Group(options.DeviceId)
                .SendAsync("ScanCompleted", result.ScanId, finalPath, cancellationToken);

            return new ScanResult
            {
                ScanId = result.ScanId,
                FilePath = finalPath,
                Success = true
            };
        }

        /// <inheritdoc/>
        /// <remarks>
        /// Devuelve <c>null</c> si no se encuentra el registro.
        /// </remarks>
        public async Task<ScanResult?> GetResultAsync(string scanId)
        {
            var record = await _context.ScanRecords
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.ScanId == scanId);

            if (record == null)
                return null;

            return new ScanResult
            {
                ScanId = record.ScanId,
                FilePath = record.FilePath,
                Success = record.Success,
                ErrorMessage = record.ErrorMessage
            };
        }

        /// <inheritdoc/>
        /// <summary>
        /// Determina si al menos un escáner está disponible para escanear.
        /// </summary>
        /// <param name="cancellationToken">Token para cancelar la comprobación.</param>
        /// <returns><c>true</c> si hay al menos un wrapper disponible; de lo contrario, <c>false</c>.</returns>
        public Task<bool> IsScannerAvailableAsync(CancellationToken cancellationToken)
        {
            bool available = _wrappers.Any(w => w.Supports(default!));
            return Task.FromResult(available);
        }
    }
}
