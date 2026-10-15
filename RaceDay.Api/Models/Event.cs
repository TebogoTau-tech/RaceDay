using System.ComponentModel.DataAnnotations;

namespace RaceDay.Api.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime EventDate { get; set; }

        [Required, MaxLength(150)]
        public string Location { get; set; } = string.Empty;

        // Foreign Key for Category
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Foreign Key linked specifically to Organiser (satisfies lecturer feedback!)
        public int OrganiserId { get; set; }
        public Organiser? Organiser { get; set; }

        // Navigation properties
        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
    }
}