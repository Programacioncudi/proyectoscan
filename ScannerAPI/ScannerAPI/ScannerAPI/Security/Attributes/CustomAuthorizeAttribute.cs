using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Security.Claims;

namespace ScannerAPI.Security.Attributes
{
    /// <summary>
    /// Filtro personalizado para autorización basado en roles.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string[] _roles;

        public CustomAuthorizeAttribute(params string[] roles)
        {
            _roles = roles;
        }

        /// <summary>
        /// Ejecuta la lógica de autorización.
        /// </summary>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? false)
            {
                context.Result = new JsonResult(new { message = "No autorizado" }) { StatusCode = 401 };
                return;
            }

            if (_roles.Any() && !_roles.Any(role => user.IsInRole(role)))
            {
                context.Result = new JsonResult(new { message = "Acceso denegado por rol" }) { StatusCode = 403 };
            }
        }
    }
}
