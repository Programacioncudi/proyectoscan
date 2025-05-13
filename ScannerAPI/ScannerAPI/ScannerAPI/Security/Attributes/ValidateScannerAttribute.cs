using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ScannerAPI.Security.Attributes
{
    /// <summary>
    /// Valida que deviceId no sea nulo o vacío.
    /// </summary>
    public class ValidateScannerAttribute : ActionFilterAttribute
    {
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
