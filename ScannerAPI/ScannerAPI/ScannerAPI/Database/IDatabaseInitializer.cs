using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScannerAPI.Models.Auth;

namespace ScannerAPI.Database
{
    /// <summary>
    /// Interfaz para inicialización de base de datos.
    /// </summary>
    public interface IDatabaseInitializer
    {
        /// <summary>
        /// Ejecuta migraciones pendientes y asegura la estructura.
        /// </summary>
        Task InitializeAsync();

        /// <summary>
        /// Carga datos por defecto en la base de datos si está vacía.
        /// </summary>
        Task SeedAsync();
    }

    /// <summary>
    /// Implementación de inicializador de base de datos que ejecuta migraciones y siembra datos iniciales.
    /// </summary>
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _context;

        public DatabaseInitializer(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task InitializeAsync()
        {
            await _context.Database.MigrateAsync();
        }

        /// <inheritdoc />
        public async Task SeedAsync()
        {
            if (!await _context.Users.AnyAsync())
            {
                var adminUser = new User
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    AccessLevel = ScannerAccessLevel.Admin,
                    LastLogin = DateTime.UtcNow
                };

                await _context.Users.AddAsync(adminUser);
                await _context.SaveChangesAsync();
            }
        }
    }
}
