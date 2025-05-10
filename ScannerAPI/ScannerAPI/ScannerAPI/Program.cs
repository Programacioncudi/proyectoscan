using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ScannerAPI.Middleware;
using ScannerAPI.Security;
using System.Text;
using ScannerAPI.Database;
using Microsoft.EntityFrameworkCore;
using NTwain;
using Microsoft.OpenApi.Models;
using ScannerAPI.Services.Interfaces;
using ScannerAPI.Services.Factories;
using ScannerAPI.Services.Implementations;
using ScannerAPI.Utilities;
using ScannerAPI.Infrastructure.Wrappers;
using ScannerAPI.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Configuración base
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ScannerAPI", Version = "v1" });

    // Configuración para JWT en Swagger
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

// Configuración JWT
var jwtConfig = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtConfig>(jwtConfig);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig["Issuer"],
            ValidAudience = jwtConfig["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtConfig["SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key is missing")))
        };

        // Configuración para SignalR
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/scannerHub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

// Configuración de autorización
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("ScannerBasic", policy =>
        policy.RequireClaim("access_level", "1", "2", "3"))
    .AddPolicy("ScannerAdvanced", policy =>
        policy.RequireClaim("access_level", "2", "3"))
    .AddPolicy("ScannerAdmin", policy =>
        policy.RequireClaim("access_level", "3"));

// Configuración de TWAIN
var twainConfig = builder.Configuration.GetSection("TwainConfig");
if (twainConfig.Exists())
{
    NTwain.TwainSession.Configure(new NTwain.TWainConfig
    {
        DSM = string.IsNullOrEmpty(twainConfig["DSMPath"]) ?
            NTwain.TWainDSM.Old :
            new NTwain.TWainDSM(twainConfig["DSMPath"]),
        CompatibilityMode = twainConfig["CompatibilityMode"] switch
        {
            "TWAIN32" => NTwain.TWainCompatibilityMode.TWAIN32,
            "TWAIN64" => NTwain.TWainCompatibilityMode.TWAIN64,
            _ => NTwain.TWainCompatibilityMode.Auto
        }
    });

    builder.Services.AddSingleton<ITwainConfig>(_ => new TwainRuntimeConfig
    {
        TransferMode = Enum.Parse<NTwain.Data.TransferMode>(twainConfig["TransferMode"] ?? "Native"),
        ShowUI = bool.Parse(twainConfig["ShowUI"] ?? "false"),
        DefaultResolution = int.Parse(twainConfig["DefaultResolution"] ?? "300"),
        MaxWaitSeconds = int.Parse(twainConfig["MaxWaitSeconds"] ?? "30")
    });
}

// Configuración de base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar servicios
builder.Services.AddSingleton<BitnessHelper>();
builder.Services.AddTransient<IImageProcessor, ImageProcessor>();
builder.Services.AddSingleton<IScannerFactory, ScannerFactory>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<IEventBusService, EventBusService>();
builder.Services.AddScoped<IScannerSessionService, ScannerSessionService>();
builder.Services.AddTransient<IEventBusService, EventBusService>();
builder.Services.AddScoped<IScannerSessionService, ScannerSessionService>();
// Registrar middleware
builder.Services.AddTransient<ErrorHandlingMiddleware>();
builder.Services.AddTransient<RequestLoggingMiddleware>();
builder.Services.AddTransient<JwtMiddleware>();
builder.Services.AddTransient<ScannerAccessMiddleware>();

var app = builder.Build();

// Configurar pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ScannerAPI v1");
        c.RoutePrefix = "api-docs";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Middleware personalizado
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<JwtMiddleware>();
app.UseMiddleware<ScannerAccessMiddleware>();

app.MapControllers();
app.MapHub<ScannerHub>("/scannerHub");

// Inicialización de base de datos
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
    await initializer.InitializeAsync();
    await initializer.SeedAsync();
}

app.Run();