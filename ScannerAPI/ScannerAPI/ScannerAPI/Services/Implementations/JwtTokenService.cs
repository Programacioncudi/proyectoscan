using Microsoft.Extensions.Options;
using ScannerAPI.Models.Auth;
using ScannerAPI.Models.Config;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Services.Implementations
{
    /// <summary>
    /// Servicio para la generación y validación de tokens JWT
    /// </summary>
    public class JwtTokenService : IJwtTokenService
    {
        private readonly JwtConfig _config;

        /// <summary>
        /// Constructor principal que recibe la configuración de JWT
        /// </summary>
        /// <param name="config">Configuración de JWT inyectada</param>
        public JwtTokenService(IOptions<JwtConfig> config)
        {
            _config = config.Value;
        }

        /// <summary>
        /// Genera un token JWT para el usuario especificado
        /// </summary>
        /// <param name="user">Usuario autenticado</param>
        /// <returns>Token JWT en formato string</returns>
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.SecretKey);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username)
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new(ClaimTypes.Role, role.ToString()));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_config.ExpiryMinutes),
                Issuer = _config.Issuer,
                Audience = _config.Audience,
                SigningCredentials = new(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}