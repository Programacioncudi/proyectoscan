# ScannerAPI

¡Bienvenidos a **ScannerAPI**! Esta aplicación ASP NET Core gestiona escaneos de documentos, usuarios, sesiones y resultados, ofreciendo autenticación JWT, límites de petición, health checks y notificaciones en tiempo real con SignalR. Está diseñada para desplegarse fácilmente con MySQL (o SQL Server), aplicar migraciones automáticas y sembrar un usuario administrador.

---

## ⚙️ Características principales

- **Autenticación y autorización** con JWT, roles (`Admin`, `User`) y políticas customizables.  
- **Gestión de usuarios** (registro, login, CRUD, hashing de contraseñas).  
- **Escaneos**: sesiones, perfiles, dispositivos y registros de cada archivo escaneado.  
- **Almacenamiento** en disco local (con `LocalScannerStorage`) y fácil extensión a otros proveedores.  
- **Rate Limiting** por IP con ventanas de tiempo configurables (middleware).  
- **Health Checks** preparados para Kubernetes o sistemas de supervisión externas.  
- **SignalR Hub** (`ScannerHub`) para notificaciones de progreso en tiempo real.  
- **Logging** completo de peticiones, errores y depuración.  
- **Swagger UI** integrado para explorar y probar la API.  
- **Inicialización automática** de base de datos: crea esquema y tablas si no existen, aplica migraciones y siembra un “admin@domain.com”.  
- **Tests** unitarios y de integración con In-Memory DB y Moq.

---

## 📁 Estructura del proyecto

/ScannerAPI
│
├─ Program.cs
│ Punto de entrada, registra servicios, middlewares, CORS, JWT, DbContext y Swagger; arranca Kestrel y ejecuta DatabaseInitializer.
│
├─ appsettings.json*
│ Configuraciones de conexión, JWT, rate limiting y logging.
│
├─ Database/
│ ├─ ApplicationDbContext.cs — EF Core DbContext con todos los DbSet<> y mapeos detallados.
│ ├─ IDatabaseInitializer.cs — interfaz para inicializar DB.
│ └─ DatabaseInitializer.cs — crea BD, aplica migraciones y siembra admin.
│
├─ Models/
│ ├─ Config/
│ │ JwtConfig.cs — POCO para bindear sección “Jwt”.
│ ├─ Api/
│ │ ApiError.cs — entidad para registrar errores (PK = Code).
│ ├─ Auth/
│ │ UserRole.cs — enum de roles.
│ │ User.cs — entidad usuario (Id, Username, Email, Roles).
│ └─ Scanner/
│ DeviceInfo.cs — dispositivo de escaneo (Id, Name).
│ ScanProfile.cs — perfil (DPI, formato, calidad).
│ ScanSession.cs — sesión activa (SessionId, UserId, timestamps).
│ ScanRecord.cs — cada archivo escaneado (ruta, éxito, error).
│
├─ Infrastructure/Storage/
│ IStorageService.cs — interfaz genérica de almacenamiento.
│ LocalScannerStorage.cs — implementación que escribe en disco.
│
├─ Services/
│ ├─ Interfaces/
│ │ IScannerService.cs — contrato para lógica de escaneo.
│ │ IAuthService.cs — registro/login.
│ │ IUserService.cs — CRUD de usuarios.
│ │ IJwtTokenService.cs — generación/validación JWT.
│ │ IDateTimeProvider.cs — fecha/hora abstracta.
│ │ IRateLimitStore.cs — almacenamiento de contadores de IP.
│ └─ Implementations/
│ ScannerService.cs — punto de partida para usar WIA o WS-Discovery.
│ AuthService.cs — maneja usuarios y tokens.
│ UserService.cs — CRUD de usuarios.
│ JwtTokenService.cs — crea JWT con SymmetricSecurityKey.
│ DateTimeProvider.cs — DateTime.UtcNow.
│ MemoryRateLimitStore.cs — contador en ConcurrentDictionary.
│
├─ Middleware/
│ ErrorHandlingMiddleware.cs — captura y formatea excepciones.
│ RequestLoggingMiddleware.cs — registra cada petición.
│
├─ RateLimiting/
│ RateLimitOptions.cs — opciones con validación de rango.
│ RateLimitingMiddleware.cs — bloquea IPs que exceden el límite.
│
├─ HealthChecks/
│ ScannerHealthCheck.cs — verifica BD y disponibilidad.
│
├─ Hubs/
│ ScannerHub.cs — hub SignalR para progreso de escaneo.
│
├─ Controllers/
│ AuthController.cs — /api/auth (register, login).
│ UsersController.cs — /api/users (CRUD).
│ ScanController.cs — /api/scan (iniciar, estado, descargar).
│ ScannersController.cs — /api/scanners (próximo: WIA/WSD).
│
├─ Utilities/
│ Constants.cs — valores globales (CORS, políticas).
│ ResponseDto.cs — wrapper genérico de respuestas.
│
├─ Migrations/
│ Archivos generados por EF Core para crear/alterar el esquema.
│
├─ Test/
│ Pruebas unitarias e integración con xUnit y Moq; usan In-Memory DB y mocks de guards.
│
├─ Dockerfile, docker-compose.yml
│ Contenedores para la DB y la propia API.
│
└─ README.md, .gitignore
Documentación y patrones de exclusión de Git.

