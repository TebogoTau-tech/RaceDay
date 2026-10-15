using Microsoft.EntityFrameworkCore;
using RaceDay.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register ApplicationDbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// --- DATABASE SEEDING FOR TESTING ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();

        // Seed a default Category if none exist
        if (!context.Categories.Any())
        {
            context.Categories.Add(new RaceDay.Api.Models.Category
            {
                Name = "10km Road Race",
                Description = "Standard 10km road running event"
            });
        }

        // Seed a default Organiser if none exist (since public registration restricts this)
        if (!context.Users.OfType<RaceDay.Api.Models.Organiser>().Any())
        {
            var organiser = new RaceDay.Api.Models.Organiser
            {
                Name = "Race Director Joe",
                Email = "organiser@raceday.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("SecurePassword123!")
            };
            context.Users.Add(organiser);
        }

        context.SaveChanges();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB or seeding data.");
    }
}
// ------------------------------------

app.Run();