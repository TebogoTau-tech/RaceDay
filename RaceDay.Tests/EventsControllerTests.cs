using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Api.Controllers;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
using Xunit;

namespace RaceDay.Tests
{
    public class EventsControllerTests
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
        public async Task GetCategories_ReturnsOkResult_WithCategories()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            context.Categories.Add(new Category { CategoryId = 1, Name = "Trail Run", Description = "Off-road running" });
            await context.SaveChangesAsync();

            var controller = new EventsController(context);

            // Act
            var result = await controller.GetCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var categories = Assert.IsAssignableFrom<IEnumerable<Category>>(okResult.Value);
            var categoryList = categories.ToList();

            Assert.Single(categoryList);
            Assert.Equal("Trail Run", categoryList[0].Name);
        }

        [Fact]
        public async Task CreateEvent_ReturnsBadRequest_WhenOrganiserNotFound()
        {
            // Arrange
            var context = GetInMemoryDbContext();
            var controller = new EventsController(context);

            var dto = new EventsController.CreateEventDto
            {
                Title = "Test Marathon",
                OrganiserId = 999 // Non-existent organiser
            };

            // Act
            var result = await controller.CreateEvent(dto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

    }
}