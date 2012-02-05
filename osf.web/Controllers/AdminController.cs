using System.Web.Mvc;
using osf.web.Data;
using osf.web.Models;
using osf.web.Services;

namespace osf.web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly EventService _eventService = new EventService();

        public ActionResult Index(int page = 1)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LatestEvent newEvent)
        {
            if (Request.Files["Image"].ContentLength == 0)
            {
                ModelState.AddModelError("Image", "Please upload an image");
            }

            if (!ModelState.IsValid)
            {
                return View(newEvent);
            }

            _eventService.AddEvent(newEvent, Request.Files["Image"]);

            return RedirectToAction("index");
        }

        public ActionResult PagedEvents(int page = 1)
        {
            return PartialView("Partials/PagedEvents", _eventService.LoadPagedEvents(page));
        }
    }
}
