using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using ScannerAPI.Database;
using ScannerAPI.Middleware;
using ScannerAPI.Security;
using ScannerAPI.Services.Interfaces;
using ScannerAPI.Services.Implementations;
using ScannerAPI.Services.Factories;
using ScannerAPI.Utilities;
using ScannerAPI.Infrastructure.Wrappers;
using ScannerAPI.Hubs;

namespace ScannerAPI
{
    var builder = WebApplication.CreateBuilder(args);

    // Configuración base
    builder.Services.AddControllers().AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
    builder.Services.AddSignalR();
    builder.Services.AddEndpointsApiExplorer();

    // Configuración de Swagger
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "ScannerAPI", Version = "v1" });

        var securityScheme = new OpenApiSecurityScheme
        {
            Name = "JWT Authentication",
            Description = "Ingrese su token JWT",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "bearer"
        };

        c.AddSecurityDefinition("Bearer", securityScheme);
        c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            { securityScheme, new string[] { } }
        });
    });

    // Base de datos
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Servicios
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IScannerService, ScannerService>();
    builder.Services.AddScoped<IScannerSessionService, ScannerSessionService>();
    builder.Services.AddScoped<IPdfService, PdfService>();
    builder.Services.AddScoped<IEventBusService, EventBusService>();
    builder.Services.AddScoped<IScannerFactory, ScannerFactory>();
    builder.Services.AddScoped<IImageProcessor, ImageProcessor>();
    builder.Services.AddScoped<IScannerWrapper, ScannerWrapperFactory>();

    builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

    // JWT
    var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

    var app = builder.Build();

    // Middleware personalizado
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<ScannerAccessMiddleware>();
    app.UseMiddleware<SignalRAuthMiddleware>();
    app.UseMiddleware<SignalRConnectionMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHub<ScannerHub>("/scannerHub");

    app.Run();
}
