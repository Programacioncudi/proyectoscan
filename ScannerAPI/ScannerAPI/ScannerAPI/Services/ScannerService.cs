using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ScannerAPI.Models.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ScannerAPI.Services
{
    /// <summary>
    /// Servicio responsable de generar y validar tokens JWT.
    /// </summary>
    public class AuthService
    {
        private readonly JwtConfig _config;

        public AuthService(IOptions<JwtConfig> config)
        {
            _config = config.Value;
        }

        /// <summary>
        /// Genera un token JWT con los claims del usuario autenticado.
        /// </summary>
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.SecretKey);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("access_level", ((int)user.AccessLevel).ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _config.Issuer,
                Audience = _config.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
