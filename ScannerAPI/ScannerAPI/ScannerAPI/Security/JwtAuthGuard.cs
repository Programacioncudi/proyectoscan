// File: Security/JwtAuthGuard.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.Text;
using ScannerAPI.Models.Config;

namespace ScannerAPI.Security
{
    /// <summary>
    /// Guardia de autorización basado en JWT para proteger acciones de controladores.
    /// </summary>
    public class JwtAuthGuard : Attribute, IAsyncAuthorizationFilter
    {
        private readonly JwtConfig _config;

        /// <summary>
        /// Crea una nueva instancia de <see cref="JwtAuthGuard"/>.
        /// </summary>
        /// <param name="config">Configuración de JWT inyectada.</param>
        public JwtAuthGuard(IOptions<JwtConfig> config)
        {
            _config = config.Value;
        }

        /// <summary>
        /// Método que se ejecuta antes de que se invoque la acción, validando el token JWT.
        /// </summary>
        /// <param name="context">Contexto de autorización del filtro.</param>
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var header = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(header) || !header.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedResult();
                return Task.CompletedTask;
            }

            var token = header.Substring("Bearer ".Length).Trim();
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config.SecretKey);
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _config.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _config.Audience,
                    ClockSkew = TimeSpan.Zero
                }, out _);
            }
            catch
            {
                context.Result = new UnauthorizedResult();
            }

            return Task.CompletedTask;
        }
    }
}
