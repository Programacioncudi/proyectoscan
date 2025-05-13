using Microsoft.AspNetCore.Authorization;
using ScannerAPI.Security.Policies;

namespace ScannerAPI.Security.Attributes
{
    /// <summary>
    /// Atributo de autorización personalizado.
    /// </summary>
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public CustomAuthorizeAttribute()
        {
            Policy = AuthorizationPolicies.UserOrAdmin;
        }
        public CustomAuthorizeAttribute(string policy)
        {
            Policy = policy;
        }
    }
}