// File: Security/Attributes/CustomAuthorizeAttribute.cs
using Microsoft.AspNetCore.Authorization;
using ScannerAPI.Security.Policies;

namespace ScannerAPI.Security.Attributes
{
    /// <summary>
    /// Atributo de autorización personalizado que aplica una política por defecto o específica.
    /// </summary>
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Crea una instancia de <see cref="CustomAuthorizeAttribute"/> con la política por defecto <c>UserOrAdmin</c>.
        /// </summary>
        public CustomAuthorizeAttribute()
        {
            Policy = AuthorizationPolicies.UserOrAdmin;
        }

        /// <summary>
        /// Crea una instancia de <see cref="CustomAuthorizeAttribute"/> con la política especificada.
        /// </summary>
        /// <param name="policy">Nombre de la política de autorización a aplicar.</param>
        public CustomAuthorizeAttribute(string policy)
        {
            Policy = policy;
        }
    }
}
