using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using ScannerAPI.Database;
using ScannerAPI.Models.Auth;
using ScannerAPI.Models.Api;
using ScannerAPI.Models.Config;
using ScannerAPI.Services.Interfaces;
using ScannerAPI.Services.Implementations;

namespace ScannerAPI.Test.Unit
{
    /// <summary>
    /// Pruebas unitarias para <see cref="AuthService"/>.
    /// </summary>
    public class AuthServiceTests
    {
        /// <summary>
        /// Verifica que <see cref="AuthService.RegisterAsync(RegisterRequest)"/>
        /// crea un usuario y devuelve un token válido.
        /// </summary>
        [Fact]
        public async Task RegisterAsync_CreatesUser_AndReturnsToken()
        {
            // Arrange
            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("AuthDb").Options;
            var context = new ApplicationDbContext(dbOptions);

            var jwtConfig = new JwtConfig
            {
                SecretKey = "supersecretkey1234567890",
                Issuer = "testIssuer",
                Audience = "testAudience"
            };
            var optionsWrapper = new OptionsWrapper<JwtConfig>(jwtConfig);

            var tokenServiceMock = new Mock<IJwtTokenService>();
            tokenServiceMock
                .Setup(t => t.GenerateToken(It.IsAny<User>()))
                .Returns("token123");

            var passwordHasher = new PasswordHasher<User>();
            var logger = Mock.Of<ILogger<AuthService>>();

            var service = new AuthService(
                optionsWrapper,
                context,
                tokenServiceMock.Object,
                passwordHasher,
                logger
            );

            // Act
            var response = await service.RegisterAsync(
                new RegisterRequest { Username = "u@x.com", Password = "password" }
            );

            // Assert
            Assert.Equal("token123", response.Token);
        }
    }
}
