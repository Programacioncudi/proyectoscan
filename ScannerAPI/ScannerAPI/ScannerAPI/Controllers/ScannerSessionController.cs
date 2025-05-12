using Microsoft.AspNetCore.Mvc;

namespace ScannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScannerSessionController : ControllerBase
    {
        /// <summary>
        /// Endpoint de prueba para la sesión del escáner.
        /// </summary>
        /// <returns>Un mensaje simple para verificar que el controlador está activo.</returns>
        [HttpGet("test")]
        public IActionResult TestSession()
        {
            return Ok("ScannerSessionController activo.");
        }
    }
}
