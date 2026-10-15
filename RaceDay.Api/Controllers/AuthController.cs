using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;
using RaceDay.Api.Models;

namespace RaceDay.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        // DTO for Participant Registration (Strictly Participants per lecturer feedback)
        public class RegisterParticipantDto
        {
            public string Name { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        // DTO for Login
        public class LoginDto
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        // POST: api/Auth/register-participant
        [HttpPost("register-participant")]
        public async Task<IActionResult> RegisterParticipant([FromBody] RegisterParticipantDto dto)
        {
            // Check if email already exists
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return BadRequest(new { message = "Email is already registered." });
            }

            // Create new Participant instance
            var participant = new Participant
            {
                Name = dto.Name,
                Email = dto.Email,
                // In a production app, hash this password using BCrypt or ASP.NET Core Identity.
                // For academic assignment scope, storing/hashing securely or via basic hash is handled here.
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(participant);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Participant registered successfully!", userId = participant.UserId });
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Unauthorized(new { message = "Invalid email or password." });
            }

            // Determine user role based on TPH discriminator
            string role = user is Organiser ? "Organiser" : "Participant";

            return Ok(new
            {
                message = "Login successful!",
                userId = user.UserId,
                name = user.Name,
                role = role
            });
        }
    }
}