// File: Controllers/HealthCheckController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading.Tasks;
using ScannerAPI.Models.Api;

namespace ScannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        private readonly HealthCheckService _healthCheckService;

        public HealthCheckController(HealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        /// <summary>
        /// Devuelve el estado de salud de la aplicación.
        /// </summary>
        /// <returns>Objeto con estado y detalles.</returns>
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
