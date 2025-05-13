// File: Models/Auth/AuthResponse.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models.Auth
{
    /// <summary>
    /// Respuesta de autenticación.
    /// </summary>
    public class AuthResponse
    {
        /// <summary>Token JWT generado.</summary>
        [Required]
        public string Token { get; set; }

        /// <summary>Fecha y hora de expiración UTC del token.</summary>
        public DateTime ExpiresAt { get; set; }
    }
}
