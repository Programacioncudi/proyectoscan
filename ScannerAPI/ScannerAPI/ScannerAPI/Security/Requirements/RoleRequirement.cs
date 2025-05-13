// Security/Requirements/RoleRequirement.cs
using System;
using Microsoft.AspNetCore.Authorization;

namespace ScannerAPI.Security.Requirements
{
    /// <summary>
    /// Requisito de autorización basado en roles.
    /// </summary>
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string[] AllowedRoles { get; }

        public RoleRequirement(params string[] allowedRoles)
        {
            if (allowedRoles == null || allowedRoles.Length == 0)
                throw new ArgumentException("Debe especificar al menos un rol", nameof(allowedRoles));
            AllowedRoles = allowedRoles;
        }
    }
}