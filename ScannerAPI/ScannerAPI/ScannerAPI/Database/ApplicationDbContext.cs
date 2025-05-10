using Microsoft.EntityFrameworkCore;
using ScannerAPI.Models.Auth;
using ScannerAPI.Models.Scanner;

namespace ScannerAPI.Database;

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
            entity.HasIndex(s => s.UserId);
            entity.Property(s => s.StartTime).HasDefaultValueSql("GETDATE()");
            entity.Property(s => s.Status).HasDefaultValue("Active");
        });

        // Configuración de ScanHistory
        modelBuilder.Entity<ScanHistory>(entity =>
        {
            entity.HasIndex(h => h.SessionId);
            entity.HasIndex(h => h.UserId);
            entity.Property(h => h.ScanDate).HasDefaultValueSql("GETDATE()");
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

            if (entityEntry.State == EntityState.Added)
            {
                ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}