namespace ScannerAPI.Models.Auth;

public class AuthResponse
{
    public string Token { get; set; }
    public int AccessLevel { get; set; }
    public DateTime ValidUntil { get; set; }
    public string RefreshToken { get; set; }
}