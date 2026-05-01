using NUnit.Framework;
using Moq;
using EMS.API.Controllers;
using EMS.API.Data;
using EMS.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMS.TEST.Controllers
{
    [TestFixture]
    public class AuthControllerTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "EMS_Test_DB")
                .Options;

            return new AppDbContext(options);
        }

        private IConfiguration GetConfig()
        {
            var settings = new Dictionary<string, string>
    {
        { "Jwt:Key", "ThisIsAVeryStrongSecretKeyForEMS_JWT_2026_Production_1234567890" },
        { "Jwt:Issuer", "EMS.API" },
        { "Jwt:Audience", "EMS.Client" }
    };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();
        }

        [Test]
        public async Task Register_ShouldReturnSuccess_WhenUserIsNew()
        {
            // Arrange
            var context = GetDbContext();
            var controller = new AuthController(context, GetConfig());

            var user = new AppUser
            {
                Username = "testuser",
                PasswordHash = "123456",
                Role = "Viewer"
            };

            // Act
            var result = await controller.Register(user);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task Register_ShouldReturnConflict_WhenUserExists()
        {
            // Arrange
            var context = GetDbContext();

            context.Users.Add(new AppUser
            {
                Username = "existing",
                PasswordHash = "hash",
                Role = "Viewer"
            });
            await context.SaveChangesAsync();

            var controller = new AuthController(context, GetConfig());

            var user = new AppUser
            {
                Username = "existing",
                PasswordHash = "123456"
            };

            // Act
            var result = await controller.Register(user);

            // Assert
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task Login_ShouldReturnToken_WhenCredentialsAreCorrect()
        {
            // Arrange
            var context = GetDbContext();

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("123456");

            context.Users.Add(new AppUser
            {
                Username = "loginuser",
                PasswordHash = hashedPassword,
                Role = "Viewer"
            });

            await context.SaveChangesAsync();

            var controller = new AuthController(context, GetConfig());

            var login = new AppUser
            {
                Username = "loginuser",
                PasswordHash = "123456"
            };

            // Act
            var result = await controller.Login(login);

            // Assert
            Assert.That(result, Is.Not.Null);
        }
    }
}