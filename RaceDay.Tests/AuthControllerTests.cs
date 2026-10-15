using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Controllers;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
using Xunit;

namespace RaceDay.Tests
{
    public class AuthControllerTests
    {
        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task RegisterParticipant_ReturnsOk_WhenValidDto()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new AuthController(context);

            var dto = new AuthController.RegisterParticipantDto
            {
                Name = "Jane Doe",
                Email = "jane@example.com",
                Password = "SecurePassword123"
            };

            // Act
            var result = await controller.RegisterParticipant(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);

            // Verify user was actually saved to the database
            var userInDb = await context.Users.FirstOrDefaultAsync(u => u.Email == "jane@example.com");
            Assert.NotNull(userInDb);
            Assert.Equal("Jane Doe", userInDb.Name);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenPasswordIsIncorrect()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword("CorrectPassword123");
            context.Users.Add(new Participant
            {
                UserId = 1,
                Name = "Test User",
                Email = "test@example.com",
                PasswordHash = hashedPassword
            });
            await context.SaveChangesAsync();

            var controller = new AuthController(context);

            var dto = new AuthController.LoginDto
            {
                Email = "test@example.com",
                Password = "WrongPassword"
            };

            // Act
            var result = await controller.Login(dto);

            // Assert
            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}