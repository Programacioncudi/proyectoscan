using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ScannerAPI.Middleware;

public class ScannerAccessMiddleware
{
    private readonly RequestDelegate _next;

    public ScannerAccessMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Aquí puedes agregar lógica de verificación de acceso al escáner
        await _next(context);
    }
}