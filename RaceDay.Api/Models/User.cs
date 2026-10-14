using Microsoft.Extensions.Logging;

namespace RaceDay.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // Security Control: Defaulted strictly to "Participant" to prevent privilege escalation.
        // Organiser roles must be administrative provisioned only.
        public string Role { get; set; } = "Participant";

        // Navigation properties
        public ICollection<Event> OrganizedEvents { get; set; } = new List<Event>();
        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
    }
}
