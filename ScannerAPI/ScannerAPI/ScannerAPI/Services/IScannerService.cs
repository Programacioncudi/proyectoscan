// File: Services/IScannerSessionService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Services
{
    /// <summary>
    /// Gestión de sesiones de escaneo.
    /// </summary>
    public interface IScannerSessionService
    {
        Task<ScanSession> CreateSessionAsync(ScanOptions options);
        Task<IEnumerable<ScanSession>> GetAllAsync();
        Task<ScanSession> GetByIdAsync(Guid sessionId);
        Task CloseSessionAsync(Guid sessionId);
    }
}
