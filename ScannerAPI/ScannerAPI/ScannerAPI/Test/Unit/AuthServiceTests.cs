using System;
using System.Threading.Tasks;
using Moq;
using Xunit;
using ScannerAPI.Services;
using ScannerAPI.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ScannerAPI.Models.Auth;
using Microsoft.Extensions.Logging;
using ScannerAPI.Services;
using ScannerAPI.Models;

namespace ScannerAPI.Tests.Unit
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task RegisterAsync_CreatesUser_AndReturnsToken()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("AuthDb").Options;
            var context = new ApplicationDbContext(options);

            var tokenServiceMock = new Mock<IJwtTokenService>();
            tokenServiceMock.Setup(t => t.GenerateToken(It.IsAny<User>()))
                .Returns("token123");

            var passwordHasher = new PasswordHasher<User>();
            var logger = Mock.Of<ILogger<AuthService>>();

            var service = new AuthService(context, tokenServiceMock.Object, passwordHasher, logger);
            var response = await service.RegisterAsync(new RegisterRequest { Username = "u@x.com", Password = "password" });
            Assert.Equal("token123", response.Token);
        }
}