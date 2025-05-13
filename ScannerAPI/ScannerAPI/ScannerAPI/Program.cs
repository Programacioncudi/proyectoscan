// File: Program.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScannerAPI.Database;
using ScannerAPI.Middleware;
using ScannerAPI.HealthChecks;
using ScannerAPI.Hubs;
using ScannerAPI.RateLimiting;
using ScannerAPI.Security.Policies;
using ScannerAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ScannerAPI.Models.Config;

var builder = WebApplication.CreateBuilder(args);

// Configuración
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servicios
builder.Services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();
builder.Services.AddSingleton<IStorageService, LocalScannerStorage>(sp =>
    new LocalScannerStorage(builder.Configuration["Storage:RootPath"], sp.GetRequiredService<ILogger<LocalScannerStorage>>()));
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<IScannerService, ScannerService>();
builder.Services.AddScoped<IScannerSessionService, ScannerSessionService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

// Autenticación
var jwtConfig = builder.Configuration.GetSection("Jwt").Get<JwtConfig>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = Encoding.ASCII.GetBytes(jwtConfig.SecretKey);
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtConfig.Audience,
            ClockSkew = TimeSpan.Zero
        };
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                    context.Token = accessToken;
                return Task.CompletedTask;
            }
        };
    });

// Autorización
builder.Services.AddAuthorization(AuthorizationPolicies.AddPolicies);
builder.Services.AddSingleton<IAuthorizationHandler, RoleRequirementHandler>();

// Salud y Rate Limiting
builder.Services.AddHealthChecks().AddCheck<ScannerHealthCheck>("scanner_health");
builder.Services.AddOptions<RateLimitOptions>().Bind(builder.Configuration.GetSection("RateLimiting")).ValidateDataAnnotations();
builder.Services.AddSingleton<IRateLimitStore, MemoryRateLimitStore>();

// CORS
builder.Services.AddCors(options => options.AddPolicy(Constants.CorsPolicyName, policy =>
{
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

// SignalR
builder.Services.AddSignalR();

// Controllers y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Inicializar BD
task.Run(async () => {
    using var scope = app.Services.CreateScope();
    var initializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
    await initializer.InitializeAsync();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(Constants.CorsPolicyName);
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseRateLimiting();

app.MapControllers();
app.MapHealthChecks("/health");
app.MapHub<ScannerHub>("/hubs/scanner");

app.Run();