
// File: Middleware/ErrorHandlingMiddleware.cs
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ScannerAPI.Models.Api;

namespace ScannerAPI.Middleware
{
    /// <summary>
    /// Middleware global para manejo de excepciones.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

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

