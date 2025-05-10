using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ScannerAPI.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ScannerAPI.Middleware;

/// <summary>
/// Middleware para validación de tokens JWT
/// </summary>
public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly JwtConfig _jwtConfig;

    public JwtMiddleware(RequestDelegate next, IOptions<JwtConfig> jwtConfig)
    {
        _next = next;
        _jwtConfig = jwtConfig.Value;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            await AttachUserToContext(context, token);
        }

        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.SecretKey);
            
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _jwtConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtConfig.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var claims = jwtToken.Claims.ToList();

            // Agregar claims personalizados
            var accessLevel = claims.FirstOrDefault(c => c.Type == "access_level")?.Value;
            if (!string.IsNullOrEmpty(accessLevel))
            {
                claims.Add(new Claim(ClaimTypes.Role, GetRoleFromAccessLevel(accessLevel)));
            }

            var identity = new ClaimsIdentity(claims, "jwt");
            context.User = new ClaimsPrincipal(identity);
        }
        catch (Exception ex)
        {
            // Token inválido - no hacemos nada, la solicitud continuará sin autenticación
            var logger = context.RequestServices.GetRequiredService<ILogger<JwtMiddleware>>();
            logger.LogWarning(ex, "Error validating JWT token");
        }
    }

    private string GetRoleFromAccessLevel(string accessLevel)
    {
        return accessLevel switch
        {
            "3" => "Admin",
            "2" => "Advanced",
            _ => "Basic"
        };
    }
}