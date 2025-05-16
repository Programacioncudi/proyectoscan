// File: Models/Api/ApiError.cs
using System.ComponentModel.DataAnnotations;

namespace ScannerAPI.Models.Api
{
    /// <summary>
    /// Detalle de un error en respuesta API.
    /// </summary>
    public class ApiError
    {
        /// <summary>Código único del error.</summary>
        [Key]
        [Required, MaxLength(50)]
        public string Code { get; set; } = string.Empty;

        /// <summary>Mensaje descriptivo del error.</summary>
        [Required]
        public string Message { get; set; } = string.Empty;
    }
}
