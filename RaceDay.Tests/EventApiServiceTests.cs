using Xunit;
using RaceDay.Web.Models;

namespace RaceDay.Tests
{
    public class EventApiServiceTests
    {
        [Fact]
        public void EventViewModel_Initialization_SetsPropertiesCorrectly()
        {
            // Arrange
            var eventModel = new EventViewModel
            {
                Title = "Cape Town Cycle Tour 2026",
                Location = "Cape Town",
                EventDate = new DateTime(2026, 12, 15)
            };

            // Act & Assert
            Assert.Equal("Cape Town Cycle Tour 2026", eventModel.Title);
            Assert.Equal("Cape Town", eventModel.Location);
            Assert.True(eventModel.EventDate > DateTime.Now);
        }

        [Fact]
        public void ImageUploadResult_StoresUrlSuccessfully()
        {
            // Arrange
            var uploadResult = new ImageUploadResult
            {
                Url = "https://racedaystorage.blob.core.windows.net/banners/sample.jpg"
            };

            // Act & Assert
            Assert.NotNull(uploadResult.Url);
            Assert.Contains("blob.core.windows.net", uploadResult.Url);
        }
    }
}
