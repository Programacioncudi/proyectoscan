// File: Database/DatabaseInitializer.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ScannerAPI.Database;
using ScannerAPI.Models.Auth;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Database
{
    /// <inheritdoc/>
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatabaseInitializer> _logger;

        public DatabaseInitializer(ApplicationDbContext context, ILogger<DatabaseInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            _logger.LogInformation("Aplicando migraciones pendientes...");
            await _context.Database.MigrateAsync();

            if (!_context.Users.Any())
            {
                _logger.LogInformation("Sembrando datos iniciales...");
                var admin = new User { Id = Guid.NewGuid(), Username = "admin@domain.com", Email = "admin@domain.com", Roles = new[] { UserRole.Admin } };
                _context.Users.Add(admin);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Usuario admin creado.");
            }
        }
    }
}