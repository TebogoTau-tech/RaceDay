namespace RaceDay.Web.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public int OrganiserId { get; set; }
    }
}
