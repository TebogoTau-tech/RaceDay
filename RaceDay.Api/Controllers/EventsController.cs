using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;
using RaceDay.Api.Services;

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

        [HttpPost("upload-banner")]
        public async Task<IActionResult> UploadBanner(IFormFile file, [FromServices] IBlobStorageService blobService)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string imageUrl = await blobService.UploadFileAsync(file, "event-banners");
            return Ok(new { Url = imageUrl });
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

        // GET: api/events/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvent(int id)
        {
            var eventItem = await _context.Events
                .Where(e => e.EventId == id)
                .Select(e => new
                {
                    Id = e.EventId,
                    e.Title,
                    e.Description,
                    e.EventDate,
                    e.Location,
                    e.CategoryId,
                    e.OrganiserId
                })
                .FirstOrDefaultAsync();

            if (eventItem == null)
            {
                return NotFound();
            }

            return Ok(eventItem);
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
                    Id =e.EventId,
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
            // Verify that the organiser exists
            var organiser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == dto.OrganiserId);
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
            var participant = await _context.Users.FirstOrDefaultAsync(p => p.UserId == dto.ParticipantId);
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

        // PUT: api/Events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event eventItem)
        {
            if (id != eventItem.EventId)
            {
                return BadRequest();
            }

            _context.Entry(eventItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Events.Any(e => e.EventId == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            // TODO: Your existing Azure Blob Storage service call here
            // e.g., string imageUrl = await _blobStorageService.UploadAsync(file);

            // For now, let's assume it returns the public URL string from Azure:
            string imageUrl = "https://yourstorageaccount.blob.core.windows.net/banners/" + file.FileName;

            return Ok(new { url = imageUrl });
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Events/enrolments/{participantId}
        [HttpGet("enrolments/{participantId}")]
        public async Task<IActionResult> GetParticipantEnrolments(int participantId)
        {
            // Check if participant exists
            var participantExists = await _context.Users
                .AnyAsync(p => p.UserId == participantId);

            if (!participantExists)
            {
                return NotFound(new { message = "Participant not found." });
            }

            var enrolments = await _context.Enrolments
                .Where(e => e.ParticipantId == participantId)
                .Include(e => e.Event)
                .Include(e => e.Category)
                .Select(e => new
                {
                    e.EnrolmentId,
                    e.EnrolmentDate,
                    EventTitle = e.Event.Title,
                    EventDate = e.Event.EventDate,
                    Location = e.Event.Location,
                    CategoryName = e.Category.Name
                })
                .ToListAsync();

            return Ok(enrolments);
        }
    }
}

