// Security/Handlers/RoleRequirementHandler.cs
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using ScannerAPI.Security.Requirements;

namespace ScannerAPI.Security.Handlers
{
    /// <summary>
    /// Handler para evaluar <see cref="RoleRequirement"/>.
    /// </summary>
    public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly ILogger<RoleRequirementHandler> _logger;

        public RoleRequirementHandler(ILogger<RoleRequirementHandler> logger)
        {
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
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