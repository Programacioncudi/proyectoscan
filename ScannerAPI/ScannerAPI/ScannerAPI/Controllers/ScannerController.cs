using Microsoft.AspNetCore.Mvc;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ScannerAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "ScannerBasic")]
public class ScannerController : ControllerBase
{
    private readonly IScannerService _scannerService;
    private readonly IScannerSessionService _sessionService;
    private readonly ILogger<ScannerController> _logger;

    public ScannerController(
        IScannerService scannerService,
        IScannerSessionService sessionService,
        ILogger<ScannerController> logger)
    {
        _scannerService = scannerService;
        _sessionService = sessionService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene la lista de dispositivos escáner disponibles
    /// </summary>
    [HttpGet("devices")]
    public async Task<ActionResult<IEnumerable<DeviceInfo>>> GetAvailableDevices()
    {
        try
        {
            var devices = await _scannerService.GetAvailableDevicesAsync();
            return Ok(devices);
        }
        catch (ScannerException ex)
        {
            _logger.LogError(ex, "Error getting scanner devices");
            return StatusCode(500, new { message = ex.Message });
        }
    }

    /// <summary>
    /// Inicia una nueva sesión de escaneo
    /// </summary>
    [HttpPost("session")]
    [Authorize(Policy = "ScannerAdvanced")]
    public async Task<ActionResult<ScanSession>> StartSession([FromBody] SessionOptions options)
    {
        try
        {
            var session = await _sessionService.CreateSessionAsync(User.Identity.Name, options);
            return Ok(session);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating scan session");
            return BadRequest(new { message = "Failed to create scan session" });
        }
    }

    /// <summary>
    /// Realiza un escaneo con las opciones especificadas
    /// </summary>
    [HttpPost("scan")]
    public async Task<ActionResult<ScanResult>> ScanDocument(
        [FromBody] ScanOptions options,
        [FromQuery] string sessionId)
    {
        try
        {
            // Validar acceso al dispositivo si es necesario
            var result = await _scannerService.ScanDocumentAsync(options, sessionId);
            return Ok(result);
        }
        catch (ScannerException ex)
        {
            _logger.LogError(ex, "Scan error for session {SessionId}", sessionId);
            return BadRequest(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized scan attempt");
            return Forbid();
        }
    }
}