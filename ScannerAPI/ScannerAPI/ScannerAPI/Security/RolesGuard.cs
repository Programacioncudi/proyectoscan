// File: Security/RolesGuardAttribute.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ScannerAPI.Models.Auth;

namespace ScannerAPI.Security
{
    /// <summary>
    /// Guardia de autorización que valida que el usuario tenga al menos uno de los roles especificados.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RolesGuardAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _roles;

        /// <summary>
        /// Crea una nueva instancia de <see cref="RolesGuardAttribute"/>.
        /// </summary>
        /// <param name="roles">Roles permitidos para acceder al recurso.</param>
        public RolesGuardAttribute(params UserRole[] roles)
        {
            _roles = roles?.Select(r => r.ToString().ToUpper()).ToArray()
                ?? throw new ArgumentNullException(nameof(roles));
        }

        /// <summary>
        /// Ejecuta la validación de roles antes de la acción del controlador.
        /// </summary>
        /// <param name="context">Contexto del filtro de autorización.</param>
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user?.Identity?.IsAuthenticated != true)
            {
                context.Result = new UnauthorizedResult();
                return Task.CompletedTask;
            }

            var roles = user.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value.ToUpper());

            if (!roles.Any(r => _roles.Contains(r)))
            {
                context.Result = new ForbidResult();
            }

            return Task.CompletedTask;
        }
    }
}
