using System.ComponentModel.DataAnnotations;

namespace RaceDay.Api.Models
{
    public class Enrolment
    {
        [Key]
        public int EnrolmentId { get; set; }

        public DateTime EnrolmentDate { get; set; } = DateTime.UtcNow;

        // Foreign Key linked specifically to Participant
        public int ParticipantId { get; set; }
        public Participant? Participant { get; set; }

        public int EventId { get; set; }
        public Event? Event { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Navigation property (1-to-1 with Result)
        public Result? Result { get; set; }
    }
}