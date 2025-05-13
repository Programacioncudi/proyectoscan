// Security/JwtAuthGuard.cs
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
    /// Guardia de autorización JWT usando atributos en controladores.
    /// </summary>
    public class JwtAuthGuard : Attribute, IAsyncAuthorizationFilter
    {
        private readonly JwtConfig _config;

        public JwtAuthGuard(IOptions<JwtConfig> config)
        {
            _config = config.Value;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var header = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(header) || !header.StartsWith("Bearer "))
            {
                context.Result = new UnauthorizedResult();
                return;
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
        }
    }
}