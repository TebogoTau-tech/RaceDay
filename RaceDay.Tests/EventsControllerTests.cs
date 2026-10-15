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
    }
}