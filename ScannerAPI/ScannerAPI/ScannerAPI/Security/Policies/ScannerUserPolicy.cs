using Microsoft.AspNetCore.Authorization;

namespace ScannerAPI.Security.Policies
{
    /// <summary>
    /// Requisito personalizado para validar el acceso como usuario de escáner.
    /// </summary>
    public class ScannerUserRequirement : IAuthorizationRequirement
    {
        public string RequiredClaim { get; } = "ScannerAccess";
    }

    /// <summary>
    /// Handler para aplicar la política ScannerUserRequirement.
    /// </summary>
    public class ScannerUserPolicy : AuthorizationHandler<ScannerUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScannerUserRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == requirement.RequiredClaim))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
