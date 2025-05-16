using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ScannerAPI.Database
{
    /// <summary>
    /// Implementaci�n de <see cref="IDatabaseInitializer"/> que aplica migraciones y semillas iniciales.
    /// </summary>
    /// <remarks>
    /// Inicializa una nueva instancia de <see cref="DatabaseInitializer"/>.
    /// </remarks>
    /// <param name="context">Contexto de base de datos de la aplicaci�n.</param>
    /// <param name="logger">Logger para registrar eventos de inicializaci�n.</param>
    public class DatabaseInitializer(ApplicationDbContext context, ILogger<DatabaseInitializer> logger) : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<DatabaseInitializer> _logger = logger;

        /// <inheritdoc/>
        public Task InitializeAsync()
            => InitializeAsync(CancellationToken.None);

        /// <summary>
        /// Ejecuta la inicializaci�n de la base de datos: aplica migraciones pendientes y realiza la siembra de datos.
        /// </summary>
        /// <param name="cancellationToken">Token que permite cancelar la operaci�n.</param>
        /// <returns>Una tarea que representa la operaci�n de inicializaci�n.</returns>
        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creando base de datos si no existe...");
            await _context.Database.EnsureCreatedAsync(cancellationToken);

            try
            {
                _logger.LogInformation("Aplicando migraciones pendientes...");
                await _context.Database.MigrateAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Se omiti� la aplicaci�n de migraciones: {Message}", ex.Message);
            }

            if (!_context.Users.Any())
            {
                _logger.LogInformation("Sembrando datos iniciales...");
                var admin = new Models.Auth.User
                {
                    Id = Guid.NewGuid(),
                    Username = "admin@domain.com",
                    Email = "admin@domain.com",
                    Roles = new[] { Models.Auth.UserRole.Admin }
                };
                _context.Users.Add(admin);
                await _context.SaveChangesAsync(cancellationToken);
                _logger.LogInformation("Usuario admin creado.");
            }
        }

    }
}
