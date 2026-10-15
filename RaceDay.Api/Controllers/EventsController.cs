using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;

namespace RaceDay.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // DTO for creating an Event
        public class CreateEventDto
        {
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public DateTime EventDate { get; set; }
            public string Location { get; set; } = string.Empty;
            public int CategoryId { get; set; }
            public int OrganiserId { get; set; }
        }

        // DTO for Enrolling in an Event
        public class EnrolmentDto
        {
            public int ParticipantId { get; set; }
            public int EventId { get; set; }
            public int CategoryId { get; set; }
        }

        // GET: api/Events
        [HttpGet]
        public async Task<IActionResult> GetEvents()
        {
            var events = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Organiser)
                .Select(e => new
                {
                    e.EventId,
                    e.Title,
                    e.Description,
                    e.EventDate,
                    e.Location,
                    Category = e.Category.Name,
                    Organiser = e.Organiser.Name
                })
                .ToListAsync();

            return Ok(events);
        }

        // GET: api/Events/categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }

        // POST: api/Events
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto dto)
        {
            // Verify that the organiser exists and is actually an Organiser
            var organiser = await _context.Users.OfType<Organiser>().FirstOrDefaultAsync(o => o.UserId == dto.OrganiserId);
            if (organiser == null)
            {
                return BadRequest(new { message = "Invalid Organiser ID. Only organisers can create events." });
            }

            // Verify category exists
            var categoryExists = await _context.Categories.AnyAsync(c => c.CategoryId == dto.CategoryId);
            if (!categoryExists)
            {
                return BadRequest(new { message = "Invalid Category ID." });
            }

            var newEvent = new Event
            {
                Title = dto.Title,
                Description = dto.Description,
                EventDate = dto.EventDate,
                Location = dto.Location,
                CategoryId = dto.CategoryId,
                OrganiserId = dto.OrganiserId
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvents), new { id = newEvent.EventId }, new { message = "Event created successfully!", eventId = newEvent.EventId });
        }

        // POST: api/Events/enrol
        [HttpPost("enrol")]
        public async Task<IActionResult> EnrolParticipant([FromBody] EnrolmentDto dto)
        {
            // Verify participant exists
            var participant = await _context.Users.OfType<Participant>().FirstOrDefaultAsync(p => p.UserId == dto.ParticipantId);
            if (participant == null)
            {
                return BadRequest(new { message = "Invalid Participant ID." });
            }

            // Verify event exists
            var raceEvent = await _context.Events.FindAsync(dto.EventId);
            if (raceEvent == null)
            {
                return NotFound(new { message = "Event not found." });
            }

            // Check if already enrolled
            var alreadyEnrolled = await _context.Enrolments
                .AnyAsync(en => en.ParticipantId == dto.ParticipantId && en.EventId == dto.EventId);

            if (alreadyEnrolled)
            {
                return BadRequest(new { message = "Participant is already enrolled in this event." });
            }

            var enrolment = new Enrolment
            {
                ParticipantId = dto.ParticipantId,
                EventId = dto.EventId,
                CategoryId = dto.CategoryId,
                EnrolmentDate = DateTime.UtcNow
            };

            _context.Enrolments.Add(enrolment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Successfully enrolled in event!", enrolmentId = enrolment.EnrolmentId });
        }
    }
}
