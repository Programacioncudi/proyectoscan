namespace ScannerAPI.Models.Auth;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public ScannerAccessLevel AccessLevel { get; set; }
    public DateTime LastLogin { get; set; }
}

public enum ScannerAccessLevel
{
    Basic = 1,
    Advanced = 2,
    Admin = 3
}