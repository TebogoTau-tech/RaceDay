using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class EnrolmentsController : Controller
    {
        private readonly ILogger<EnrolmentsController> _logger;
        // Inject your API service here if you have one set up for enrolments
        // private readonly EnrolmentApiService _enrolmentApiService;

        public EnrolmentsController(ILogger<EnrolmentsController> logger)
        {
            _logger = logger;
        }

        // GET: Enrolments/MyEvents
        public async Task<IActionResult> MyEvents()
        {
            // Get the current logged-in participant's ID from session
            int? participantId = HttpContext.Session.GetInt32("UserId");

            // If you want mock/test data for now while building the view, or call your API:
            //var enrolments = await _enrolmentApiService.GetEnrolmentsForParticipantAsync(participantId ?? 1);

            return View(); // We will pass your enrolment model here
        }
    }
}