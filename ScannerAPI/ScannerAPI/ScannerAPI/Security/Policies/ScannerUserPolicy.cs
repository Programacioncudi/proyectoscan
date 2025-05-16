// File: Security/Policies/ScannerUserPolicy.cs
using Microsoft.AspNetCore.Authorization;

namespace ScannerAPI.Security.Policies
{
    /// <summary>
    /// Pol�tica de autorizaci�n para usuarios que pueden acceder al esc�ner.
    /// Permite roles <c>USER</c> y <c>ADMIN</c>.
    /// </summary>
    public static class ScannerUserPolicy
    {
        /// <summary>
        /// Nombre de la pol�tica que valida roles de usuario o administrador.
        /// </summary>
        public const string PolicyName = "ScannerUser";

        /// <summary>
        /// Agrega la pol�tica <c>ScannerUser</c> a las opciones de autorizaci�n.
        /// </summary>
        /// <param name="options">Opciones de autorizaci�n a configurar.</param>
        public static void AddPolicy(AuthorizationOptions options)
        {
            options.AddPolicy(PolicyName, policy =>
                policy.RequireAssertion(ctx =>
                    ctx.User.HasClaim(c =>
                        c.Type == "role" && (c.Value == "USER" || c.Value == "ADMIN"))));
        }
    }
}
