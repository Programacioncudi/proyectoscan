// File: Controllers/ScannerController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using System.Threading.Tasks;
using ScannerAPI.Services;
using ScannerAPI.Models.Api;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ScannerController : ControllerBase
    {
        private readonly IScannerService _scannerService;

        public ScannerController(IScannerService scannerService)
        {
            _scannerService = scannerService;
        }

        /// <summary>
        /// Inicia un escaneo con las opciones especificadas.
        /// </summary>
        [HttpPost("scan")]
        [ProducesResponseType(typeof(ApiResponse<ScanResult>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 400)]
        public async Task<IActionResult> Scan([FromBody] ScanOptions options, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse<object> { Success = false, Error = new ApiError { Code = "InvalidOptions", Message = "Opciones de escaneo inválidas." } });

            var result = await _scannerService.ScanAsync(options, cancellationToken);
            return Ok(new ApiResponse<ScanResult> { Success = true, Data = result });
        }

        /// <summary>
        /// Obtiene el resultado de un escaneo por ID.
        /// </summary>
        [HttpGet("{scanId}")]
        [ProducesResponseType(typeof(ApiResponse<ScanResult>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetResult(string scanId)
        {
            var result = await _scannerService.GetResultAsync(scanId);
            if (result == null)
                return NotFound(new ApiResponse<object> { Success = false, Error = new ApiError { Code = "NotFound", Message = "Resultado no encontrado." } });

            return Ok(new ApiResponse<ScanResult> { Success = true, Data = result });
        }
    }
}