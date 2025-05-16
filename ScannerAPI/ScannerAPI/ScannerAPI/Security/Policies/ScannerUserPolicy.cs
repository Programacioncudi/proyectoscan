// File: Security/Policies/ScannerUserPolicy.cs
using Microsoft.AspNetCore.Authorization;

namespace ScannerAPI.Security.Policies
{
    /// <summary>
    /// Política de autorización para usuarios que pueden acceder al escáner.
    /// Permite roles <c>USER</c> y <c>ADMIN</c>.
    /// </summary>
    public static class ScannerUserPolicy
    {
        /// <summary>
        /// Nombre de la política que valida roles de usuario o administrador.
        /// </summary>
        public const string PolicyName = "ScannerUser";

        /// <summary>
        /// Agrega la política <c>ScannerUser</c> a las opciones de autorización.
        /// </summary>
        /// <param name="options">Opciones de autorización a configurar.</param>
        public static void AddPolicy(AuthorizationOptions options)
        {
            options.AddPolicy(PolicyName, policy =>
                policy.RequireAssertion(ctx =>
                    ctx.User.HasClaim(c =>
                        c.Type == "role" && (c.Value == "USER" || c.Value == "ADMIN"))));
        }
    }
}
