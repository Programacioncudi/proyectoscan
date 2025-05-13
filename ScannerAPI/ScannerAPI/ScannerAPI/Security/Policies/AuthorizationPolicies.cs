// Security/Policies/AuthorizationPolicies.cs
using Microsoft.AspNetCore.Authorization;

namespace ScannerAPI.Security.Policies
{
    /// <summary>
    /// Políticas de autorización centralizadas.
    /// </summary>
    public static class AuthorizationPolicies
    {
        public const string AdminOnly = "AdminOnly";
        public const string UserOrAdmin = "UserOrAdmin";

        public static void AddPolicies(AuthorizationOptions options)
        {
            options.AddPolicy(AdminOnly, policy => policy.RequireRole("ADMIN"));
            options.AddPolicy(UserOrAdmin, policy => policy.RequireRole("ADMIN", "USER"));
        }
    }
}