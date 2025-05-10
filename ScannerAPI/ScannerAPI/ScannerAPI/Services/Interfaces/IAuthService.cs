namespace ScannerAPI.Services.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwtToken(string userId);
        bool ValidateToken(string token);
    }
}