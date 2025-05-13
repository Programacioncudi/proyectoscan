using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using ScannerAPI.Services;
using ScannerAPI.Services.Interfaces;
using ScannerAPI.Models.Scanner;
using ScannerAPI.Database;
using ScannerAPI.Hubs;

namespace ScannerAPI.Tests.Unit
{
    public class ScannerServiceTests
    {
        [Fact]
        public async Task ScanAsync_ReturnsResult_WhenWrapperSupports()
        {
            var options = new ScanOptions { DeviceId = "dev1", Dpi = 100, Format = FileFormat.PNG };
            var wrapperMock = new Mock<IScannerWrapper>();
            wrapperMock.Setup(w => w.Supports(options)).Returns(true);
            wrapperMock.Setup(w => w.ScanAsync(options, It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ScanResult { ScanId = "123", FilePath = "/tmp/1.png", Success = true });

            var storageMock = new Mock<IStorageService>();
            storageMock.Setup(s => s.SaveFileAsync(It.IsAny<string>(), It.IsAny<byte[]>()))
                .ReturnsAsync("/perm/1.png");

            var optionsDb = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb").Options;
            var context = new ApplicationDbContext(optionsDb);

            var hubMock = new Mock<IHubContext<ScannerHub>>();

            var service = new ScannerService(
                new[] { wrapperMock.Object },
                storageMock.Object,
                context,
                hubMock.Object);

            var result = await service.ScanAsync(options, CancellationToken.None);
            Assert.True(result.Success);
            Assert.Equal("123", result.ScanId);
        }
    }
}