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
            var badRequestResult =  Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task EnrolParticipant_ReturnsBadRequest_WhenAlreadyEnrolled()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            // Seed test data matching your actual model properties
            var participant = new Participant { UserId = 1, Name = "John Doe", Email = "john@example.com", PasswordHash = "hash" };
            var category = new Category { CategoryId = 1, Name = "Marathon", Description = "Full marathon" };
            var organiser = new Organiser { UserId = 2, Name = "Org", Email = "org@example.com", PasswordHash = "hash" };

            var raceEvent = new Event
            {
                EventId = 1,
                Title = "Spring Run",
                Description = "Fun run",
                EventDate = DateTime.UtcNow.AddDays(5),
                Location = "Park",
                CategoryId = 1,
                OrganiserId = 2
            };

            context.Users.Add(participant);
            context.Users.Add(organiser);
            context.Categories.Add(category);
            context.Events.Add(raceEvent);

            // Add an initial enrolment
            context.Enrolments.Add(new Enrolment { EnrolmentId = 1, ParticipantId = 1, EventId = 1, CategoryId = 1, EnrolmentDate = DateTime.UtcNow });
            await context.SaveChangesAsync();

            var controller = new EventsController(context);

            var dto = new EventsController.EnrolmentDto
            {
                ParticipantId = 1,
                EventId = 1,
                CategoryId = 1
            };

            // Act
            var result = await controller.EnrolParticipant(dto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
        }

        [Fact]
        public async Task GetParticipantEnrolments_ReturnsOk_WithUserEnrolments()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            var participant = new Participant { UserId = 1, Name = "John Doe", Email = "john@example.com", PasswordHash = "hash" };
            var category = new Category { CategoryId = 1, Name = "Marathon", Description = "Full marathon" };
            var organiser = new Organiser { UserId = 2, Name = "Org", Email = "org@example.com", PasswordHash = "hash" };

            var raceEvent = new Event
            {
                EventId = 1,
                Title = "Spring Run",
                Description = "Fun run",
                EventDate = DateTime.UtcNow.AddDays(5),
                Location = "Park",
                CategoryId = 1,
                OrganiserId = 2
            };

            context.Users.Add(participant);
            context.Users.Add(organiser);
            context.Categories.Add(category);
            context.Events.Add(raceEvent);

            context.Enrolments.Add(new Enrolment
            {
                EnrolmentId = 1,
                ParticipantId = 1,
                EventId = 1,
                CategoryId = 1,
                EnrolmentDate = DateTime.UtcNow
            });

            await context.SaveChangesAsync();

            var controller = new EventsController(context);

            // Act
            var result = await controller.GetParticipantEnrolments(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

    }
}