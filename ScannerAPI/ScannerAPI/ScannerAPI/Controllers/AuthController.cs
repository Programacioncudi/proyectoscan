using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using ScannerAPI.Services;
using ScannerAPI.Models.Api;
using ScannerAPI.Models.Auth;
using ScannerAPI.Models;
using ScannerAPI.Exceptions;

namespace ScannerAPI.Controllers
{
    /// <summary>
    /// Controlador que expone los endpoints de autenticación: registro e inicio de sesión.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        /// <summary>
        /// Constructor del controlador de autenticación.
        /// </summary>
        /// <param name="authService">Servicio de autenticación inyectado.</param>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Registra un nuevo usuario en el sistema.
        /// </summary>
        /// <param name="dto">Datos de registro.</param>
        /// <returns>Token de autenticación y fecha de expiración.</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<AuthResponse>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object> { Success = false, Error = new ApiError { Code = "InvalidData", Message = "Datos de registro inválidos." } });

            try
            {
                var result = await _authService.RegisterAsync(dto);
                return Ok(new ApiResponse<AuthResponse> { Success = true, Data = result });
            }
            catch (DomainException ex)
            {
                return BadRequest(new ApiResponse<object> { Success = false, Error = new ApiError { Code = ex.Code, Message = ex.Message } });
            }
        }

        /// <summary>
        /// Autentica un usuario existente y genera un token JWT.
        /// </summary>
        /// <param name="dto">Credenciales de login.</param>
        /// <returns>Token JWT y fecha de expiración.</returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<AuthResponse>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 401)]
        public async Task<IActionResult> Login([FromBody] LoginRequest dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object> { Success = false, Error = new ApiError { Code = "InvalidData", Message = "Datos de login inválidos." } });

            try
            {
                var result = await _authService.LoginAsync(dto);
                return Ok(new ApiResponse<AuthResponse> { Success = true, Data = result });
            }
            catch (DomainException ex)
            {
                return Unauthorized(new ApiResponse<object> { Success = false, Error = new ApiError { Code = ex.Code, Message = ex.Message } });
            }
        }
    }
}
