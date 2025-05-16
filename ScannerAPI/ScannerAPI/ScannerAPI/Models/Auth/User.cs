// File: Models/Auth/User.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models.Auth
{
    /// <summary>
    /// Representa un usuario del sistema.
    /// </summary>
    public class User
    {
        /// <summary>Identificador único.</summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>Nombre de usuario.</summary>
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        /// <summary>Email del usuario.</summary>
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        /// <summary>Roles asignados.</summary>
        public ICollection<UserRole> Roles { get; set; } = new List<UserRole>();

        /// <summary>Hash de la contraseña.</summary>
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
