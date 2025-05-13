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
        public string Username { get; set; }

        /// <summary>Email del usuario.</summary>
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        /// <summary>Roles asignados.</summary>
        public ICollection<UserRole> Roles { get; set; }
    }
}
