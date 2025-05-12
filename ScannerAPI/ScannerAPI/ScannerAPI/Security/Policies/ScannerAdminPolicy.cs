using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ScannerAPI.Security.Policies
{
    /// <summary>
    /// Requisito para validar que el usuario tenga rol de administrador de escaneo.
    /// </summary>
    public class ScannerAdminRequirement : IAuthorizationRequirement { }

    /// <summary>
    /// Handler para la política de administrador de escaneo.
    /// </summary>
    public class ScannerAdminHandler : AuthorizationHandler<ScannerAdminRequirement>
    {
        /// <summary>
        /// Verifica si el usuario tiene el rol de "ScannerAdmin".
        /// </summary>
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ScannerAdminRequirement requirement)
        {
            var roleClaim = context.User.FindFirst(ClaimTypes.Role);

            if (roleClaim != null && roleClaim.Value == "ScannerAdmin")
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
