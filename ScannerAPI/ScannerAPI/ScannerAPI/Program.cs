using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ScannerAPI.Database;
using ScannerAPI.HealthChecks;
using ScannerAPI.Hubs;
using ScannerAPI.Middleware;
using ScannerAPI.Models.Config;
using ScannerAPI.RateLimiting;
using ScannerAPI.Security.Handlers;
using ScannerAPI.Security.Policies;
using ScannerAPI.Services.Implementations;
using ScannerAPI.Services.Interfaces;
using ScannerAPI.Infrastructure.Storage;
using ScannerAPI.Services;
using ScannerAPI.Utilities;
using Microsoft.AspNetCore.Identity;
using ScannerAPI.Models.Auth;
var builder = WebApplication.CreateBuilder(args);

// 1. Configuración de JWT y DbContext
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));

// Program.cs

// 1. Leer la cadena de conexión y verificarla
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' no encontrada.");

// 2. Registrar el DbContext pasando siempre un string no-null
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseMySQL(connectionString)
);
// 2. Registración de servicios core
builder.Services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
builder.Services.AddSingleton<IStorageService>(sp =>
{
    var path = builder.Configuration["Storage:RootPath"]
               ?? throw new InvalidOperationException("Storage:RootPath no configurado");
    var logger = sp.GetRequiredService<ILogger<LocalScannerStorage>>();
    return new LocalScannerStorage(path, logger);
});
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();

// 3. Registración de servicios de dominio
builder.Services.AddScoped<IScannerService, ScannerService>();
builder.Services.AddScoped<IScannerSessionService, ScannerSessionService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();



// Registrar el PasswordHasher para tu entidad User:
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<IUserService, UserService>();


// 4. JWT Bearer Authentication
var jwtCfg = builder.Configuration.GetSection("Jwt").Get<JwtConfig>()
             ?? throw new InvalidOperationException("Sección Jwt no encontrada");
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        var key = Encoding.ASCII.GetBytes(jwtCfg.SecretKey);
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtCfg.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtCfg.Audience,
            ClockSkew = TimeSpan.Zero
        };
        opt.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                var token = ctx.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(token) && ctx.HttpContext.Request.Path.StartsWithSegments("/hubs/scanner"))
                    ctx.Token = token;
                return Task.CompletedTask;
            }
        };
    });

// 5. Authorization
builder.Services.AddAuthorization(AuthorizationPolicies.AddPolicies);
builder.Services.AddSingleton<IAuthorizationHandler, RoleRequirementHandler>();

// 6. Health Checks
builder.Services.AddHealthChecks()
       .AddCheck<ScannerHealthCheck>("scanner_health");

// 7. Rate Limiting
builder.Services.AddOptions<RateLimitOptions>()
       .Bind(builder.Configuration.GetSection("RateLimiting"))
       .ValidateDataAnnotations();
builder.Services.AddSingleton<IRateLimitStore, MemoryRateLimitStore>();

// 8. CORS
builder.Services.AddCors(o =>
    o.AddPolicy(Constants.CorsPolicyName, p =>
        p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    ));

// 9. MVC, SignalR, Swagger
builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 10. Inicialización de la base de datos al inicio
using (var scope = app.Services.CreateScope())
{
    var init = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
    await init.InitializeAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
//app.UseHttpsRedirection();
app.UseRouting();

app.UseCors(Constants.CorsPolicyName);
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();

app.MapControllers();
app.MapHealthChecks("/health");
app.MapHub<ScannerHub>("/hubs/scanner");

await app.RunAsync();
