// Security/RolesGuard.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ScannerAPI.Models.Auth;

namespace ScannerAPI.Security
{
    /// <summary>
    /// Guardia para roles específicos en controladores.
    /// </summary>
    public class RolesGuardAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _roles;

        public RolesGuardAttribute(params UserRole[] roles)
        {
            _roles = roles.Select(r => r.ToString().ToUpper()).ToArray();
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return Task.CompletedTask;
            }

            var roles = user.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value);
            if (!roles.Any(r => _roles.Contains(r.ToUpper())))
            {
                context.Result = new ForbidResult();
            }
            return Task.CompletedTask;
        }
    }
}