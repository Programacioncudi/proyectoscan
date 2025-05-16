// File: Security/Requirements/RoleRequirement.cs
using System;
using Microsoft.AspNetCore.Authorization;

namespace ScannerAPI.Security.Requirements
{
    /// <summary>
    /// Representa un requisito de autorización que verifica si el usuario tiene alguno de los roles permitidos.
    /// </summary>
    public class RoleRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Roles permitidos para satisfacer este requisito.
        /// </summary>
        public string[] AllowedRoles { get; }

        /// <summary>
        /// Crea una nueva instancia de <see cref="RoleRequirement"/> con los roles especificados.
        /// </summary>
        /// <param name="allowedRoles">Lista de roles que cumplen el requisito.</param>
        /// <exception cref="ArgumentException">Si no se especifica al menos un rol.</exception>
        public RoleRequirement(params string[] allowedRoles)
        {
            if (allowedRoles == null || allowedRoles.Length == 0)
                throw new ArgumentException("Debe especificar al menos un rol", nameof(allowedRoles));
            AllowedRoles = allowedRoles;
        }
    }
}
