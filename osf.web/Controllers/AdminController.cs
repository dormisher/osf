using System.Drawing;
using System.Web;
using System.Web.Mvc;
using osf.web.Data;
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
        public ActionResult Index(LatestEvent newEvent)
        {
            ValidateAddEventImage();

            if (!ModelState.IsValid)
            {
            	ViewBag.Page = 1;
                return View(newEvent);
            }

            _eventService.AddEvent(newEvent, Request.Files["image"]);

            return RedirectToAction("index");
        }
		
        public ActionResult PagedEvents(int page = 1)
        {
            return PartialView("Partials/PagedEvents", _eventService.LoadPagedEvents(page));
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
		public ActionResult Edit(LatestEvent latestEvent)
		{
			ValidateEditEventImage();

			if (!ModelState.IsValid)
			{
				ViewBag.Page = 1;
				return View(latestEvent);
			}

			_eventService.Edit(latestEvent, Request.Files["image"]);

			return RedirectToAction("index");
		}

		// private validation methods etc

		private void ValidateEditEventImage()
		{
			HttpPostedFileBase file = Request.Files["image"];

			if (file.ContentLength > 0)
			{
				ValidateImageSize(file);
			}
		}

        private void ValidateAddEventImage()
        {
            HttpPostedFileBase file = Request.Files["image"];

            if (file == null ||file.ContentLength == 0)
            {
                ModelState.AddModelError("image", "Please upload an image");
            }
            else
            {
            	ValidateImageSize(file);
            }
        }

		private void ValidateImageSize(HttpPostedFileBase file)
		{
			var image = Image.FromStream(file.InputStream, true, true);

			if (image.Width < 620)
			{
				ModelState.AddModelError("image", "Image must be at least 620px wide");
			}
			else if (image.Height < 300)
			{
				ModelState.AddModelError("image", "Image must be at least 300px high");
			}
		}
    }
}
