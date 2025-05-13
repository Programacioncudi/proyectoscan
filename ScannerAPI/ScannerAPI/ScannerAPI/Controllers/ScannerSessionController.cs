// File: Controllers/ScannerSessionController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using ScannerAPI.Services;
using ScannerAPI.Models.Api;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ScannerSessionController : ControllerBase
    {
        private readonly IScannerSessionService _sessionService;

        public ScannerSessionController(IScannerSessionService sessionService)
        {
            _sessionService = sessionService;
        }

        /// <summary>
        /// Crea una nueva sesión de escaneo.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ScanSessionDto>), 201)]
        public async Task<IActionResult> Create([FromBody] ScanOptions options)
        {
            var session = await _sessionService.CreateSessionAsync(options);
            return CreatedAtAction(nameof(GetById), new { sessionId = session.SessionId }, new ApiResponse<ScanSessionDto> { Success = true, Data = session });
        }

        /// <summary>
        /// Obtiene todas las sesiones.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<ScanSessionDto[]>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var sessions = await _sessionService.GetAllAsync();
            return Ok(new ApiResponse<ScanSessionDto[]> { Success = true, Data = sessions.ToArray() });
        }

        /// <summary>
        /// Obtiene una sesión por ID.
        /// </summary>
        [HttpGet("{sessionId}")]
        [ProducesResponseType(typeof(ApiResponse<ScanSessionDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetById(Guid sessionId)
        {
            var session = await _sessionService.GetByIdAsync(sessionId);
            if (session == null)
                return NotFound(new ApiResponse<object> { Success = false, Error = new ApiError { Code = "NotFound", Message = "Sesión no encontrada." } });

            return Ok(new ApiResponse<ScanSessionDto> { Success = true, Data = session });
        }

        /// <summary>
        /// Cierra una sesión de escaneo.
        /// </summary>
        [HttpDelete("{sessionId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> Close(Guid sessionId)
        {
            await _sessionService.CloseSessionAsync(sessionId);
            return NoContent();
        }
    }
}
