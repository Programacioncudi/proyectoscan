// File: Security/Attributes/ValidateScannerAttribute.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ScannerAPI.Security.Attributes
{
    /// <summary>
    /// Valida que el parámetro <c>deviceId</c> no sea nulo o vacío antes de ejecutar la acción.
    /// </summary>
    public class ValidateScannerAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Se ejecuta antes de la acción para verificar el parámetro <c>deviceId</c>.
        /// </summary>
        /// <param name="context">Contexto de ejecución del filtro.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("deviceId", out var value))
            {
                var deviceId = value as string;
                if (string.IsNullOrWhiteSpace(deviceId))
                {
                    context.Result = new BadRequestObjectResult("deviceId es requerido.");
                }
            }
            base.OnActionExecuting(context);
        }
    }
}
