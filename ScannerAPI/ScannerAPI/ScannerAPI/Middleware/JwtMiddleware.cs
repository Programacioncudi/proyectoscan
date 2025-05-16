// File: Middleware/JwtMiddleware.cs
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ScannerAPI.Models.Config;

namespace ScannerAPI.Middleware
{
    /// <summary>
    /// Extrae y valida el JWT en cada petición.
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtConfig _config;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="JwtMiddleware"/>.
        /// </summary>
        /// <param name="next">Delegado al siguiente middleware en la canalización.</param>
        /// <param name="config">Configuración para validación de JWT.</param>
        public JwtMiddleware(RequestDelegate next, IOptions<JwtConfig> config)
        {
            _next = next;
            _config = config.Value;
        }

        /// <summary>
        /// Invoca el middleware, extrae y valida el token JWT si está presente.
        /// </summary>
        /// <param name="context">Contexto HTTP de la petición.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"]
                             .FirstOrDefault()?
                             .Split(' ')
                             .Last();
            if (token != null)
                AttachUserToContext(context, token);

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var key = System.Text.Encoding.ASCII.GetBytes(_config.SecretKey);
                handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _config.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _config.Audience,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "id").Value;
                context.Items["UserId"] = userId;
            }
            catch
            {
                // Si el token es inválido o expiró, no adjuntamos usuario al contexto
            }
        }
    }
}
