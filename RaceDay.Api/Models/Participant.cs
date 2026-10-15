namespace RaceDay.Api.Models
{
    public class Participant : User
    {
        // Navigation property for events the participant registers for
        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
    }
}
