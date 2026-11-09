using Microsoft.AspNetCore.Mvc;
using RaceDay.Web.Models;
using RaceDay.Web.Services;

namespace RaceDay.Web.Controllers
{
    public class EventsController : Controller
    {
        private readonly EventApiService _eventApiService;

        public EventsController(EventApiService eventApiService)
        {
            _eventApiService = eventApiService;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var events = await _eventApiService.GetEventsAsync();
            return View(events);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var eventItem = await _eventApiService.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return View(eventItem);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel model)
        {
            if (ModelState.IsValid)
            {
                var success = await _eventApiService.CreateEventAsync(model);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Unable to create event. Please try again.");
            }
            return View(model);
        }

        // GET: Events/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var eventItem = await _eventApiService.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return View(eventItem);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var success = await _eventApiService.UpdateEventAsync(id, model);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "Unable to update the event. Please try again.");
            }
            return View(model);
        }

        // GET: Events/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var eventItem = await _eventApiService.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return View(eventItem);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _eventApiService.DeleteEventAsync(id);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Delete), new { id, saveError = true });
        }
    }
}