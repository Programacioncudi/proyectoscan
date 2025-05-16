using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using ScannerAPI.Models.Api;


namespace ScannerAPI.Controllers
{
    /// <summary>
    /// Controlador que expone el endpoint de verificación de salud de la aplicación.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        /// <summary>
        /// Constructor del controlador de health check.
        /// </summary>
        /// <param name="healthCheckService">Servicio de comprobación de salud inyectado.</param>
        public HealthCheckController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        /// <summary>
        /// Obtiene el estado de salud de la aplicación.
        /// </summary>
        /// <returns>ApiResponse con el HealthReport detallado.</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<HealthReport>), 200)]
        public async Task<IActionResult> Get()
        {
            var report = await _healthCheckService.CheckHealthAsync();
            return Ok(new ApiResponse<HealthReport> { Success = true, Data = report });
        }
    }
}

