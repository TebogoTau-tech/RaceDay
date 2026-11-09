using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using RaceDay.Web.Models;

namespace RaceDay.Web.Services
{
    public class EventApiService
    {
        private readonly HttpClient _httpClient;

        public EventApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("RaceDayApi");
        }

        public async Task<IEnumerable<EventViewModel>> GetEventsAsync()
        {
            var events = await _httpClient.GetFromJsonAsync<IEnumerable<EventViewModel>>("events");
            return events ?? Enumerable.Empty<EventViewModel>();
        }

        public async Task<string?> UploadBannerAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            using var content = new MultipartFormDataContent();

            // Read the file stream
            using var streamContent = new StreamContent(file.OpenReadStream());
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            // Attach it to the package with the name "file" (must match the API parameter name)
            content.Add(streamContent, "file", file.FileName);

            // Send it to your API upload endpoint
            var response = await _httpClient.PostAsync("api/events/upload-image", content);

            if (response.IsSuccessStatusCode)
            {
                // Read the returned JSON result containing the Azure URL
                var result = await response.Content.ReadFromJsonAsync<ImageUploadResult>();
                return result?.Url;
            }

            return null;
        }

        public async Task<EventViewModel?> GetEventByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<EventViewModel>($"events/{id}");
        }

        public async Task<bool> CreateEventAsync(EventViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("events", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEventAsync(int id, EventViewModel model)
        {
            var response = await _httpClient.PutAsJsonAsync($"events/{id}", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"events/{id}");
            return response.IsSuccessStatusCode;
        }
    }

    // Helper model for parsing the uploaded image response from the API
    public class ImageUploadResult
    {
        public string Url { get; set; } = string.Empty;
    }
}