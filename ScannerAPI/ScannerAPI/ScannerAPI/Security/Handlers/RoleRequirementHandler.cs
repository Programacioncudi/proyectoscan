// File: Security/Handlers/RoleRequirementHandler.cs
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ScannerAPI.Security.Requirements;

namespace ScannerAPI.Security.Handlers
{
    /// <summary>
    /// Handler que evalúa el requisito de roles definidos en <see cref="RoleRequirement"/>.
    /// </summary>
    public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly ILogger<RoleRequirementHandler> _logger;

        /// <summary>
        /// Crea una nueva instancia de <see cref="RoleRequirementHandler"/>.
        /// </summary>
        /// <param name="logger">Logger para registrar eventos de autorización.</param>
        public RoleRequirementHandler(ILogger<RoleRequirementHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Maneja la validación del requisito de roles.
        /// </summary>
        /// <param name="context">Contexto del handler de autorización.</param>
        /// <param name="requirement">Requisito que contiene los roles permitidos.</param>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (context.User?.Identity?.IsAuthenticated != true)
            {
                _logger.LogWarning("Usuario no autenticado.");
                context.Fail();
                return Task.CompletedTask;
            }

            var roles = context.User.FindAll(ClaimTypes.Role).Select(c => c.Value);
            if (roles.Any(r => requirement.AllowedRoles.Contains(r)))
            {
                context.Succeed(requirement);
            }
            else
            {
                _logger.LogWarning("Usuario {User} sin roles permitidos.", context.User.Identity.Name);
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
