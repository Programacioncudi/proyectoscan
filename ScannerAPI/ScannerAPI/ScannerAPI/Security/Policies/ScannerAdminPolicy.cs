// File: Security/Policies/ScannerAdminPolicy.cs
using Microsoft.AspNetCore.Authorization;

namespace ScannerAPI.Security.Policies
{
    /// <summary>
    /// Política de autorización específica para administradores del escáner.
    /// </summary>
    public static class ScannerAdminPolicy
    {
        /// <summary>
        /// Nombre de la política que aplica la validación de rol <c>ADMIN</c>.
        /// </summary>
        public const string PolicyName = "ScannerAdmin";

        /// <summary>
        /// Agrega la política <c>ScannerAdmin</c> a las opciones de autorización.
        /// </summary>
        /// <param name="options">Opciones de autorización a configurar.</param>
        public static void AddPolicy(AuthorizationOptions options)
        {
            options.AddPolicy(PolicyName, policy =>
                policy.RequireClaim("role", "ADMIN"));
        }
    }
}
