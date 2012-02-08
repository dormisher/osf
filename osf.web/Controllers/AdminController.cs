using System.Drawing;
using System.Web;
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
            ViewBag.Page = page;

            return View();
        }

        [HttpPost]
        public ActionResult Index(LatestEventModel newEvent)
        {
            var image = Request.Files["image"];

            _eventService.ValidateAddEventImage(image, ModelState);
            _eventService.ValidateEvent(newEvent, ModelState);

            if (!ModelState.IsValid)
            {
            	ViewBag.Page = 1;
                return View(newEvent);
            }

            _eventService.AddEvent(newEvent, image);

            return RedirectToAction("index");
        }
		
        public ActionResult PagedEvents(int page = 1)
        {
            return PartialView("Partials/PagedEvents", _eventService.LoadPagedEvents(page, 5));
        }

		public ActionResult Delete(int id)
		{
			_eventService.Delete(id);

			return RedirectToAction("index");
		}

		public ActionResult Edit(int id)
		{
			return View(_eventService.Load(id));
		}

		[HttpPost]
        public ActionResult Edit(LatestEventModel latestEvent)
		{
		    var image = Request.Files["image"];

			_eventService.ValidateEditEventImage(image, ModelState);
            _eventService.ValidateEvent(latestEvent, ModelState);

			if (!ModelState.IsValid)
			{
				ViewBag.Page = 1;
				return View(latestEvent);
			}

			_eventService.Edit(latestEvent, image);

			return RedirectToAction("index");
		}
    }
}
