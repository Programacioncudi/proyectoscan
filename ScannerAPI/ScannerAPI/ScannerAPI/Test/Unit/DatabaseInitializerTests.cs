using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using ScannerAPI.Database;
using ScannerAPI.Models.Auth;

namespace ScannerAPI.Tests.Unit
{
    public class DatabaseInitializerTests
    {
        [Fact]
        public async Task InitializeAsync_SeedsAdmin_WhenNoUsers()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("InitDb").Options;
            var context = new ApplicationDbContext(options);
            var logger = Mock.Of<ILogger<DatabaseInitializer>>();
            var init = new DatabaseInitializer(context, logger);

            await init.InitializeAsync();
            Assert.Single(context.Users.ToList());
            var user = context.Users.First();
            Assert.Contains(UserRole.Admin, user.Roles);
        }
    }
}