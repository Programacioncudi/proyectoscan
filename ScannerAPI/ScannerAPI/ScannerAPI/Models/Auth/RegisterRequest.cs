// File: Models/Auth/RegisterRequest.cs
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models.Auth
{
    /// <summary>
    /// Datos para registrar un nuevo usuario.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>Email o nombre de usuario.</summary>
        [Required]
        [EmailAddress]
        public string Username { get; set; } = string.Empty;

        /// <summary>Contraseña (mínimo 8 caracteres).</summary>
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
}
