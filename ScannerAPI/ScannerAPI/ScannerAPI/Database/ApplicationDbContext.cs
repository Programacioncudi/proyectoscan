// File: Database/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using ScannerAPI.Models;
using ScannerAPI.Models.Api;
using ScannerAPI.Models.Auth;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Database
{
    /// <summary>
    /// Contexto de base de datos de la aplicación, alberga los DbSet de las entidades.
    /// </summary>
    /// <remarks>
    /// Inicializa una nueva instancia de <see cref="ApplicationDbContext"/> con las opciones especificadas.
    /// </remarks>
    /// <param name="options">Opciones de configuración del DbContext.</param>
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {

        /// <summary>Conjunto de usuarios registrados.</summary>
        public DbSet<User> Users { get; set; }
        /// <summary>Conjunto de sesiones de escaneo.</summary>
        public DbSet<ScanSession> ScanSessions { get; set; }
        /// <summary>Conjunto de registros de escaneos realizados.</summary>
        public DbSet<ScanRecord> ScanRecords { get; set; }
        /// <summary>Conjunto de perfiles de escaneo predefinidos.</summary>
        public DbSet<ScanProfile> ScanProfiles { get; set; }
        /// <summary>Conjunto de dispositivos disponibles.</summary>
        public DbSet<DeviceInfo> Devices { get; set; }

        /// <summary>Conjunto de errores API almacenados.</summary>
        public DbSet<ApiError> ApiErrors { get; set; }

        /// <summary>
        /// Configura el modelo y las convenciones de entidades al construir el modelo.
        /// </summary>
        /// <param name="modelBuilder">Constructor del modelo.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- User ---
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.HasIndex(u => u.Email)
                      .IsUnique()
                      .HasDatabaseName("IX_Users_Email");
                // Roles es una colección de enum, se almacena como JSON o se ignora la configuración relacional.
                entity.Ignore(u => u.Roles);
            });

            // --- ScanProfile ---
            modelBuilder.Entity<ScanProfile>(entity =>
            {
                entity.ToTable("ScanProfiles");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(p => p.Dpi)
                      .IsRequired();
                entity.Property(p => p.Format)
                      .IsRequired()
                      .HasMaxLength(10);
                entity.Property(p => p.Quality)
                      .IsRequired();
                entity.HasIndex(p => p.Name)
                      .IsUnique()
                      .HasDatabaseName("IX_ScanProfiles_Name");
            });

            // --- DeviceInfo ---
            modelBuilder.Entity<DeviceInfo>(entity =>
            {
                entity.ToTable("Devices");
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Id)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(d => d.Name)
                      .IsRequired()
                      .HasMaxLength(200);
            });

            // --- ScanSession ---
            modelBuilder.Entity<ScanSession>(entity =>
            {
                entity.ToTable("ScanSessions");
                entity.HasKey(s => s.SessionId);
                entity.Property(s => s.ScanId)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(s => s.StartedAt)
                      .IsRequired();
                entity.Property(s => s.CompletedAt);
                entity.HasOne<User>()
                      .WithMany()
                      .HasForeignKey(s => s.UserId)
                      .OnDelete(DeleteBehavior.Restrict)
                      .HasConstraintName("FK_ScanSessions_Users");
            });

            // --- ScanRecord ---
            modelBuilder.Entity<ScanRecord>(entity =>
            {
                entity.ToTable("ScanRecords");
                entity.HasKey(r => r.Id);
                entity.Property(r => r.ScanId)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(r => r.FilePath)
                      .IsRequired()
                      .HasMaxLength(500);
                entity.Property(r => r.Success)
                      .IsRequired();
                entity.Property(r => r.ErrorMessage)
                      .HasMaxLength(1000);
                entity.HasOne<ScanSession>()
                      .WithMany()
                      .HasForeignKey(r => r.SessionId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK_ScanRecords_ScanSessions");
            });

            // --- Convenciones de nombres ---
            // Ejemplo: aplicar snake_case si se desea
        }
    }
}
