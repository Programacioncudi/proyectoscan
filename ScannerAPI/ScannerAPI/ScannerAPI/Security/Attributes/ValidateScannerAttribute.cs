using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using ScannerAPI.Services.Interfaces;

namespace ScannerAPI.Security.Attributes
{
    /// <summary>
    /// Filtro que valida que el escáner esté disponible antes de ejecutar la acción.
    /// </summary>
    public class ValidateScannerAttribute : ActionFilterAttribute
    {
        private readonly ILogger<ValidateScannerAttribute> _logger;
        private readonly IScannerService _scannerService;

        public ValidateScannerAttribute(
            ILogger<ValidateScannerAttribute> logger,
            IScannerService scannerService)
        {
            _logger = logger;
            _scannerService = scannerService;
        }

        /// <summary>
        /// Valida si hay escáneres disponibles antes de ejecutar la acción.
        /// </summary>
        /// <param name="context">Contexto de ejecución de acción.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var task = _scannerService.GetAvailableDevicesAsync();
            task.Wait();
            var devices = task.Result;

            if (devices == null || devices.Count == 0)
            {
                _logger.LogWarning("No hay escáneres disponibles para esta operación.");
                context.Result = new BadRequestObjectResult("No hay escáneres disponibles.");
            }

            base.OnActionExecuting(context);
        }
    }
}
