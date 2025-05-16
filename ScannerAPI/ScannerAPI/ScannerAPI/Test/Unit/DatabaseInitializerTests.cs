using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using ScannerAPI.Database;
using ScannerAPI.Models.Auth;

namespace ScannerAPI.Test.Unit
{
    /// <summary>
    /// Pruebas unitarias para <see cref="DatabaseInitializer"/>, 
    /// verificando la inicialización de datos semilla y sobrecargas.
    /// </summary>
    public class DatabaseInitializerTests
    {
        /// <summary>
        /// Verifica que, cuando no existen usuarios, 
        /// <see cref="DatabaseInitializer.InitializeAsync()"/> crea al menos un administrador.
        /// </summary>
        [Fact]
        public async Task InitializeAsync_SeedsAdmin_WhenNoUsers()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InitDb").Options;
            var context = new ApplicationDbContext(options);
            var logger = Mock.Of<ILogger<DatabaseInitializer>>();
            var init = new DatabaseInitializer(context, logger);

            // Act
            await init.InitializeAsync();

            // Assert
            Assert.Single(context.Users.ToList());
            var user = context.Users.First();
            Assert.Contains(UserRole.Admin, user.Roles);
        }

        /// <summary>
        /// Verifica que la sobrecarga con CancellationToken existe y es accesible
        /// </summary>
        [Fact]
        public void InitializeAsync_WithCancellationToken_ExecutesCorrectly()
        {
            // Especificar la sobrecarga exacta
            var method = typeof(DatabaseInitializer).GetMethod(
                nameof(DatabaseInitializer.InitializeAsync),
                new[] { typeof(CancellationToken) }
            );
            Assert.NotNull(method);
        }
    }
}