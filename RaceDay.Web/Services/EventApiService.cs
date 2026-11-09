using System.Net.Http.Json;
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
            var events = await _httpClient.GetFromJsonAsync<IEnumerable<EventViewModel>>("api/events");
            return events ?? Enumerable.Empty<EventViewModel>();
        }

        public async Task<EventViewModel?> GetEventByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<EventViewModel>($"api/events/{id}");
        }

        public async Task<bool> CreateEventAsync(EventViewModel model)
        {
            var response = await _httpClient.PostAsJsonAsync("api/events", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateEventAsync(int id, EventViewModel model)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/events/{id}", model);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteEventAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/events/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}