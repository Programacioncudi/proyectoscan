// File: Middleware/ErrorHandlingMiddleware.cs
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ScannerAPI.Models.Api;
using ScannerAPI.Exceptions;

namespace ScannerAPI.Middleware
{
    /// <summary>
    /// Middleware global para manejo de excepciones y respuestas de error.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        /// <summary>
        /// Inicializa una nueva instancia de <see cref="ErrorHandlingMiddleware"/>.
        /// </summary>
        /// <param name="next">Delegado para la siguiente pieza de middleware en la canalización de solicitudes.</param>
        /// <param name="logger">Logger para registrar eventos y excepciones.</param>
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invoca el middleware, captura excepciones y escribe la respuesta de error.
        /// </summary>
        /// <param name="context">HttpContext de la petición actual.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Error de dominio");
                await WriteErrorAsync(context, HttpStatusCode.BadRequest, ex.Code, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Acceso no autorizado");
                await WriteErrorAsync(context, HttpStatusCode.Unauthorized, "Unauthorized", "Acceso no autorizado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado");
                await WriteErrorAsync(context, HttpStatusCode.InternalServerError, "ServerError", "Ocurrió un error interno.");
            }
        }

        private static async Task WriteErrorAsync(HttpContext context, HttpStatusCode statusCode, string code, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new ApiResponse<object>
            {
                Success = false,
                Error = new ApiError { Code = code, Message = message }
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
