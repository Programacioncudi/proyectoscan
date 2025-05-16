// File: Security/Policies/AuthorizationPolicies.cs
using Microsoft.AspNetCore.Authorization;

namespace ScannerAPI.Security.Policies
{
    /// <summary>
    /// Políticas de autorización centralizadas para la aplicación.
    /// </summary>
    public static class AuthorizationPolicies
    {
        /// <summary>
        /// Política que solo permite acceso a usuarios con el rol <c>ADMIN</c>.
        /// </summary>
        public const string AdminOnly = "AdminOnly";

        /// <summary>
        /// Política que permite acceso a usuarios con rol <c>USER</c> o <c>ADMIN</c>.
        /// </summary>
        public const string UserOrAdmin = "UserOrAdmin";

        /// <summary>
        /// Extiende las opciones de autorización agregando las políticas definidas.
        /// </summary>
        /// <param name="options">Opciones de autorización a configurar.</param>
        public static void AddPolicies(AuthorizationOptions options)
        {
            options.AddPolicy(AdminOnly, policy => policy.RequireRole("ADMIN"));
            options.AddPolicy(UserOrAdmin, policy => policy.RequireRole("ADMIN", "USER"));
        }
    }
}
