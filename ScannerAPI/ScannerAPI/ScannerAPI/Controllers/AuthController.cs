using Microsoft.AspNetCore.Mvc;
using ScannerAPI.Models.Auth;
using ScannerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ScannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Autentica un usuario y genera un token JWT con los permisos correspondientes
    /// </summary>
    /// <param name="request">Credenciales de acceso</param>
    /// <returns>Respuesta con token JWT y nivel de acceso</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var response = await _authService.AuthenticateAsync(request.Username, request.Password);
            _logger.LogInformation("Login successful for user: {Username}", request.Username);
            return Ok(response);
        }
        catch (AuthenticationException ex)
        {
            _logger.LogWarning("Authentication failed for user: {Username}", request.Username);
            return Unauthorized(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Verifica si el token actual es válido y devuelve información del usuario
    /// </summary>
    [HttpGet("validate")]
    [Authorize]
    public IActionResult ValidateToken()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        var accessLevel = User.FindFirst("access_level")?.Value;

        return Ok(new {
            UserId = userId,
            Username = username,
            AccessLevel = accessLevel,
            IsValid = true
        });
    }
}