// File: Services/ScannerSessionService.cs
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScannerAPI.Database;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Services
{
    /// <inheritdoc/>
    public class ScannerSessionService : IScannerSessionService
    {
        private readonly ApplicationDbContext _context;

        public ScannerSessionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ScanSession> CreateSessionAsync(ScanOptions options)
        {
            var session = new ScanSession
            {
                SessionId = Guid.NewGuid(),
                ScanId = Guid.NewGuid().ToString(),
                StartedAt = DateTime.UtcNow,
                DeviceId = options.DeviceId,
                UserId = Guid.Parse((string)options.GetType()
                    .GetProperty("UserId")?
                    .GetValue(options) ?? "00000000-0000-0000-0000-000000000000" )
            };
            _context.ScanSessions.Add(session);
            await _context.SaveChangesAsync();
            return session;
        }

        public async Task<IEnumerable<ScanSession>> GetAllAsync()
        {
            return await _context.ScanSessions.ToListAsync();
        }

        public async Task<ScanSession> GetByIdAsync(Guid sessionId)
        {
            return await _context.ScanSessions.FindAsync(sessionId);
        }

        public async Task CloseSessionAsync(Guid sessionId)
        {
            var session = await _context.ScanSessions.FindAsync(sessionId);
            if (session == null)
                throw new DomainException("NotFound", "Sesión no encontrada.");

            session.CompletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
