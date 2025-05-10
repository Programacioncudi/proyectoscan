using ScannerAPI.Database;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Services.Implementations;

public class ScannerSessionService : IScannerSessionService
{
    private readonly ApplicationDbContext _context;

    public ScannerSessionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ScanSession> CreateSessionAsync(string username, SessionOptions options)
    {
        var session = new ScanSession
        {
            Id = Guid.NewGuid().ToString(),
            UserId = username,
            DeviceId = options.DeviceId,
            Status = "Active"
        };

        await _context.ScanSessions.AddAsync(session);
        await _context.SaveChangesAsync();

        return session;
    }

    public Task<ScanSession> GetSessionAsync(string sessionId)
    {
        return _context.ScanSessions.FindAsync(sessionId).AsTask();
    }

    public async Task EndSessionAsync(string sessionId)
    {
        var session = await _context.ScanSessions.FindAsync(sessionId);
        if (session != null)
        {
            session.Status = "Completed";
            await _context.SaveChangesAsync();
        }
    }
}