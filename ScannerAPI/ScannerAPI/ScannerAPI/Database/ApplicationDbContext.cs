using Microsoft.EntityFrameworkCore;
using ScannerAPI.Models.Auth;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Database
{
    /// <summary>
    /// Contexto de base de datos principal para la aplicación ScannerAPI.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {
        }

        // Tablas de la base de datos
        public DbSet<User> Users { get; set; }
        public DbSet<ScanSession> ScanSessions { get; set; }
        public DbSet<DeviceInfo> Devices { get; set; }
        public DbSet<ScanHistory> ScanHistory { get; set; }

        /// <summary>
        /// Configura las entidades del modelo al crear el esquema de base de datos.
        /// </summary>
        /// <param name="modelBuilder">Builder de modelo para Entity Framework.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.Username).IsUnique();
                entity.Property(u => u.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(u => u.AccessLevel).HasDefaultValue(ScannerAccessLevel.Basic);
            });

            // Configuración de ScanSession
            modelBuilder.Entity<ScanSession>(entity =>
            {
                entity.HasIndex(s => s.SessionId).IsUnique();
                entity.Property(s => s.StartedAt).HasDefaultValueSql("GETDATE()");
            });

            // Configuración de ScanHistory
            modelBuilder.Entity<ScanHistory>(entity =>
            {
                entity.Property(h => h.ScannedAt).HasDefaultValueSql("GETDATE()");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

