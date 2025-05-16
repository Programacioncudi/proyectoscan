// File: Security/Policies/ScannerAdminPolicy.cs
using Microsoft.AspNetCore.Authorization;

namespace ScannerAPI.Security.Policies
{
    /// <summary>
    /// Pol�tica de autorizaci�n espec�fica para administradores del esc�ner.
    /// </summary>
    public static class ScannerAdminPolicy
    {
        /// <summary>
        /// Nombre de la pol�tica que aplica la validaci�n de rol <c>ADMIN</c>.
        /// </summary>
        public const string PolicyName = "ScannerAdmin";

        /// <summary>
        /// Agrega la pol�tica <c>ScannerAdmin</c> a las opciones de autorizaci�n.
        /// </summary>
        /// <param name="options">Opciones de autorizaci�n a configurar.</param>
        public static void AddPolicy(AuthorizationOptions options)
        {
            options.AddPolicy(PolicyName, policy =>
                policy.RequireClaim("role", "ADMIN"));
        }
    }
}
