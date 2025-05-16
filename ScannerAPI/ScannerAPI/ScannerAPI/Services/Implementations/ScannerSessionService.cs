// File: Services/Implementations/ScannerSessionService.cs
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScannerAPI.Database;
using ScannerAPI.Exceptions;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Services.Interfaces;
using SessionOptionsModel = ScannerAPI.Models.Scanner.SessionOptions;

namespace ScannerAPI.Services.Implementations
{
    /// <inheritdoc />
    public class ScannerSessionService : IScannerSessionService
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Crea una nueva instancia de <see cref="ScannerSessionService"/>.
        /// </summary>
        /// <param name="context">Contexto de la base de datos.</param>
        public ScannerSessionService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc />
        public async Task<ScanSession> CreateSessionAsync(string username, SessionOptionsModel options)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new DomainException("InvalidInput", "Username es requerido.");
            if (options == null)
                throw new DomainException("InvalidInput", "SessionOptions es requerido.");
            if (string.IsNullOrWhiteSpace(options.DeviceId))
                throw new DomainException("InvalidInput", "DeviceId es requerido.");

            var session = new ScanSession
            {
                SessionId = Guid.NewGuid(),
                ScanId = Guid.NewGuid().ToString(),
                StartedAt = DateTime.UtcNow,
                DeviceId = options.DeviceId,
                UserId = Guid.Empty, // ajustar según cómo resuelvas el usuario real
                UserName = username
            };

            _context.ScanSessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        /// <inheritdoc />
        public async Task<ScanSession> GetSessionAsync(string sessionId)
        {
            if (!Guid.TryParse(sessionId, out var guid))
                throw new DomainException("InvalidInput", "SessionId no válido.");

            var session = await _context.ScanSessions
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.SessionId == guid);

            return session
                ?? throw new DomainException("NotFound", "Sesión no encontrada.");
        }

        /// <inheritdoc />
        public async Task EndSessionAsync(string sessionId)
        {
            if (!Guid.TryParse(sessionId, out var guid))
                throw new DomainException("InvalidInput", "SessionId no válido.");

            var session = await _context.ScanSessions.FindAsync(guid);
            if (session == null)
                throw new DomainException("NotFound", "Sesión no encontrada.");

            session.CompletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
        /// <inheritdoc />
        public async Task<IEnumerable<ScanSession>> GetAllSessionsAsync()
        {
            // Trae todas las sesiones de la BD sin tracking para optimizar lectura
            return await _context.ScanSessions
                                 .AsNoTracking()
                                 .ToListAsync();
        }
    }
}
