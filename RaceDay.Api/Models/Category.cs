using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace RaceDay.Api.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty; // e.g., 10km, 21km, 42km Marathon

        public string Description { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Event> Events { get; set; } = new List<Event>();
        public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
    }
}