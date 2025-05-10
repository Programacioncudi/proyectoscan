using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Services.Interfaces;

public interface IScannerSessionService
{
    Task<ScanSession> CreateSessionAsync(string username, SessionOptions options);
    Task<ScanSession> GetSessionAsync(string sessionId);
    Task EndSessionAsync(string sessionId);
}