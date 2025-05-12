using Microsoft.AspNetCore.Mvc;

namespace ScannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {
        /// <summary>
        /// Verifica si la API está activa y funcionando.
        /// </summary>
        /// <returns>Un mensaje simple con código 200.</returns>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("OK");
        }
    }
}
