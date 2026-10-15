using Microsoft.Extensions.Logging;

namespace RaceDay.Api.Models
{
    public class Organiser : User
    {
        // Navigation property for events created by this organiser
        public ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();
    }
}