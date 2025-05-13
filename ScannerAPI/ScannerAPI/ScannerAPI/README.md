# ScannerAPI

API REST para escaneo de documentos usando WIA y TWAIN, con autenticación JWT, SignalR y rate limiting.

## Estructura del proyecto
- **Controllers/**: Endpoints API
- **Services/**: Lógica de negocio
- **Utilities/**: Helpers y wrappers
- **Infrastructure/**: Implementaciones de wrappers
- **Security/**: JWT, políticas, middleware
- **Tests/**: Unit e Integration tests
- **wwwroot/**: Cliente web de ejemplo
- **Config/**: JSON de configuración

## Cómo ejecutar
```bash
dotnet build
dotnet run --project ScannerAPI/ScannerAPI.csproj