using Microsoft.AspNetCore.Mvc;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace ScannerAPI.Controllers
{
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
        /// Obtiene la lista de dispositivos escáner disponibles.
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
                return StatusCode(500, new { message = "Error al obtener los dispositivos de escaneo." });
            }
        }
    }
}
