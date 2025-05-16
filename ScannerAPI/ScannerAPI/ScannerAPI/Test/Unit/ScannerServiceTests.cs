// File: Tests/Unit/ScannerServiceTests.cs
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using ScannerAPI.Database;
using ScannerAPI.Hubs;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Services.Implementations;
using ScannerAPI.Services.Interfaces;
using ScannerAPI.Services;

namespace ScannerAPI.Tests.Unit
{
    /// <summary>
    /// Pruebas unitarias para <see cref="ScannerService"/>.
    /// </summary>
    public class ScannerServiceTests
    {
        /// <summary>
        /// Verifica que <see cref="ScannerService.ScanAsync(ScanOptions, CancellationToken)"/>
        /// retorne un resultado exitoso cuando existe un wrapper que soporta las opciones.
        /// </summary>
        [Fact]
        public async Task ScanAsync_ReturnsResult_WhenWrapperSupports()
        {
            // Arrange
            var options = new ScanOptions { DeviceId = "dev1", Dpi = 100, Format = FileFormat.Png };
            var wrapperMock = new Mock<IScannerWrapper>();
            wrapperMock
                .Setup(w => w.Supports(options))
                .Returns(true);
            wrapperMock
                .Setup(w => w.ScanAsync(options, It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ScanResult
                {
                    ScanId = "123",
                    FilePath = "/tmp/1.png",
                    Success = true
                });

            var storageMock = new Mock<IStorageService>();
            storageMock
                .Setup(s => s.SaveFileAsync(It.IsAny<string>(), It.IsAny<byte[]>()))
                .ReturnsAsync("/perm/1.png");

            var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .Options;
            var context = new ApplicationDbContext(dbOptions);

            var hubMock = new Mock<IHubContext<ScannerHub>>();

            var service = new ScannerService(
                new[] { wrapperMock.Object },
                storageMock.Object,
                context,
                hubMock.Object);

            // Act
            var result = await service.ScanAsync(options, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("123", result.ScanId);
        }
    }
}
