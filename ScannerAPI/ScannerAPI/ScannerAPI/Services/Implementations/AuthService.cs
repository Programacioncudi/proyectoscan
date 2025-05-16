using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ScannerAPI.Database;
using ScannerAPI.Models.Auth;
using ScannerAPI.Models.Config;
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Services.Implementations // Namespace corregido
{
    /// <summary>
    /// Servicio de autenticación con generación de tokens JWT
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly JwtConfig _config;
        private readonly ApplicationDbContext _db;
        private readonly IJwtTokenService _tokenSvc;
        private readonly IPasswordHasher<User> _hasher;
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// Constructor principal con todas las dependencias requeridas
        /// </summary>
        public AuthService(
            IOptions<JwtConfig> config,
            ApplicationDbContext db,
            IJwtTokenService tokenSvc,
            IPasswordHasher<User> hasher,
            ILogger<AuthService> logger)
        {
            _config = config.Value;
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _tokenSvc = tokenSvc ?? throw new ArgumentNullException(nameof(tokenSvc));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Registra un nuevo usuario y devuelve un JWT
        /// </summary>
        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("Username y Password son obligatorios");
            }

            var user = new User
            {
                Username = request.Username,
                Roles = new[] { UserRole.User }
            };

            user.PasswordHash = _hasher.HashPassword(user, request.Password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var token = _tokenSvc.GenerateToken(user);
            return new AuthResponse
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_config.ExpiryMinutes)
            };
        }

        /// <summary>
        /// Autentica un usuario y genera un token JWT
        /// </summary>
        public async Task<AuthResponse> LoginAsync(LoginRequest dto)
        {
            var user = await _db.Users
                .FirstOrDefaultAsync(u => u.Username == dto.Username)
                ?? throw new UnauthorizedAccessException("Credenciales inválidas");

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result != PasswordVerificationResult.Success)
                throw new UnauthorizedAccessException("Credenciales inválidas");

            var token = _tokenSvc.GenerateToken(user);
            return new AuthResponse
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddMinutes(_config.ExpiryMinutes)
            };
        }

        // Eliminar el método GenerateToken redundante (ya está en JwtTokenService)
    }
}