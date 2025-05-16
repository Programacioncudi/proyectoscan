// File: Models/Auth/LoginRequest.cs
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models.Auth
{
    /// <summary>
    /// Credenciales para autenticarse.
    /// </summary>
    public class LoginRequest
    {
        /// <summary>Email del usuario.</summary>
        [Required]
        [EmailAddress]
        public string Username { get; set; } = string.Empty;

        /// <summary>Contraseña del usuario (mínimo 8 caracteres).</summary>
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;
    }
}
