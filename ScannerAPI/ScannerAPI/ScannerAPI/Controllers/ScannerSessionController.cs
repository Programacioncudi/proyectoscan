using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;
using ScannerAPI.Models.Api;
using ScannerAPI.Models.Dtos;
using ScannerAPI.Services.Interfaces;
using ScannerAPI.Exceptions;
using SessionOptions = ScannerAPI.Models.Scanner.SessionOptions;

namespace ScannerAPI.Controllers
{
    /// <summary>
    /// Controlador que gestiona las sesiones de escaneo: creación, consulta y cierre.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ScannerSessionController : ControllerBase
    {
        private readonly IScannerSessionService _sessionService;

        /// <summary>
        /// Constructor del controlador de sesiones de escaneo.
        /// </summary>
        /// <param name="sessionService">Servicio de gestión de sesiones de escaneo inyectado.</param>
        public ScannerSessionController(IScannerSessionService sessionService)
        {
            _sessionService = sessionService;
        }

        /// <summary>
        /// Crea una nueva sesión de escaneo para el usuario autenticado.
        /// </summary>
        /// <param name="options">Opciones de configuración de la sesión.</param>
        /// <returns>Detalles de la sesión creada.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<ScanSessionDto>), 201)]
        public async Task<IActionResult> Create([FromBody] SessionOptions options)
        {
            var username = User.Identity?.Name ?? throw new InvalidOperationException("Usuario no autenticado");
            var domainSession = await _sessionService.CreateSessionAsync(username, options);
            var dto = new ScanSessionDto
            {
                SessionId = domainSession.SessionId.ToString(),
                DeviceId = domainSession.DeviceId,
                StartedAt = domainSession.StartedAt,
                EndedAt = domainSession.CompletedAt
            };
            return CreatedAtAction(
                nameof(GetById),
                new { sessionId = dto.SessionId },
                new ApiResponse<ScanSessionDto> { Success = true, Data = dto }
            );
        }

        /// <summary>
        /// Obtiene todas las sesiones de escaneo.
        /// </summary>
        /// <returns>Listado de sesiones existentes.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<ScanSessionDto[]>), 200)]
        public async Task<IActionResult> GetAll()
        {
            var domainList = await _sessionService.GetAllSessionsAsync();
            var dtos = domainList.Select(s => new ScanSessionDto
            {
                SessionId = s.SessionId.ToString(),
                DeviceId = s.DeviceId,
                StartedAt = s.StartedAt,
                EndedAt = s.CompletedAt
            }).ToArray();
            return Ok(new ApiResponse<ScanSessionDto[]>
            {
                Success = true,
                Data = dtos
            });
        }

        /// <summary>
        /// Obtiene una sesión de escaneo por su identificador.
        /// </summary>
        /// <param name="sessionId">Identificador único de la sesión.</param>
        /// <returns>Detalles de la sesión o error 404 si no existe.</returns>
        [HttpGet("{sessionId}")]
        [ProducesResponseType(typeof(ApiResponse<ScanSessionDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> GetById(string sessionId)
        {
            var domainSession = await _sessionService.GetSessionAsync(sessionId);
            if (domainSession == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Error = new ApiError { Code = "NotFound", Message = "Sesión no encontrada." }
                });
            }
            var dto = new ScanSessionDto
            {
                SessionId = domainSession.SessionId.ToString(),
                DeviceId = domainSession.DeviceId,
                StartedAt = domainSession.StartedAt,
                EndedAt = domainSession.CompletedAt
            };
            return Ok(new ApiResponse<ScanSessionDto> { Success = true, Data = dto });
        }

        /// <summary>
        /// Marca una sesión de escaneo como finalizada.
        /// </summary>
        /// <param name="sessionId">Identificador único de la sesión a cerrar.</param>
        /// <returns>NoContent si cierra correctamente o 404 si no existe.</returns>
        [HttpDelete("{sessionId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(typeof(ApiResponse<object>), 404)]
        public async Task<IActionResult> Close(string sessionId)
        {
            try
            {
                await _sessionService.EndSessionAsync(sessionId);
                return NoContent();
            }
            catch (DomainException ex) when (ex.Code == "NotFound")
            {
                return NotFound(new ApiResponse<object>
                {
                    Success = false,
                    Error = new ApiError { Code = ex.Code, Message = ex.Message }
                });
            }
        }
    }
}
