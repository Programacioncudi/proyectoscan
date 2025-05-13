// File: Services/ScannerService.cs
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ScannerAPI.Database;
using ScannerAPI.Hubs;
using ScannerAPI.Infrastructure.Storage;
using ScannerAPI.Infrastructure.Wrappers;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Services
{
    /// <inheritdoc/>
    public class ScannerService : IScannerService
    {
        private readonly IEnumerable<IScannerWrapper> _wrappers;
        private readonly IStorageService _storage;
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<ScannerHub> _hubContext;

        public ScannerService(
            IEnumerable<IScannerWrapper> wrappers,
            IStorageService storage,
            ApplicationDbContext context,
            IHubContext<ScannerHub> hubContext)
        {
            _wrappers = wrappers;
            _storage = storage;
            _context = context;
            _hubContext = hubContext;
        }

        public async Task<ScanResult> ScanAsync(ScanOptions options, CancellationToken cancellationToken)
        {
            var wrapper = _wrappers.FirstOrDefault(w => w.Supports(options));
            if (wrapper == null)
                throw new DomainException("NoScanner", "No se encontró un escáner compatible.");

            var fileName = $"{Guid.NewGuid()}.{options.Format}".ToLower();
            var tempPath = Path.Combine(Path.GetTempPath(), fileName);

            var result = await wrapper.ScanAsync(options, tempPath, cancellationToken);
            var finalPath = await _storage.SaveFileAsync(fileName, await File.ReadAllBytesAsync(tempPath, cancellationToken));

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

            await _hubContext.Clients.Group(options.DeviceId)
                .SendAsync("ScanCompleted", result.ScanId, finalPath);

            return new ScanResult { ScanId = result.ScanId, FilePath = finalPath, Success = true };
        }

        public async Task<ScanResult> GetResultAsync(string scanId)
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
    }
}


}