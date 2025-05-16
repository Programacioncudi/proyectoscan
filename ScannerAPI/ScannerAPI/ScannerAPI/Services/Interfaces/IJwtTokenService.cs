// File: Services/Interfaces/IJwtTokenService.cs
namespace ScannerAPI.Services.Interfaces
{
    /// <summary>
    /// Servicio para generación de JWT a partir de un usuario.
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// Genera un token JWT para el usuario dado.
        /// </summary>
        string GenerateToken(ScannerAPI.Models.Auth.User user);
    }
}
