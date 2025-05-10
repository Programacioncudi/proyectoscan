using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ScannerAPI.Database;
using ScannerAPI.Models.Auth;
using ScannerAPI.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ScannerAPI.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly JwtConfig _jwtConfig;
    private readonly ApplicationDbContext _context;

    public AuthService(IOptions<JwtConfig> jwtConfig, ApplicationDbContext context)
    {
        _jwtConfig = jwtConfig.Value;
        _context = context;
    }

    public async Task<AuthResponse> AuthenticateAsync(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            throw new Exception("Invalid credentials");

        var token = GenerateJwtToken(user);
        
        return new AuthResponse
        {
            Token = token,
            AccessLevel = user.AccessLevel
        };
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtConfig.SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("access_level", ((int)user.AccessLevel).ToString())
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtConfig.ExpiryMinutes),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }
}