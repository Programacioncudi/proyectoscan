// File: Services/IAuthService.cs
using System.Threading.Tasks;
using ScannerAPI.Models.Auth;

namespace ScannerAPI.Services
{
    /// <summary>
    /// Lógica de negocio para autenticación de usuarios.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>Registra un nuevo usuario y retorna token de acceso.</summary>
        Task<AuthResponse> RegisterAsync(RegisterRequest dto);

        /// <summary>Valida credenciales y retorna token de acceso.</summary>
        Task<AuthResponse> LoginAsync(LoginRequest dto);
    }
}
