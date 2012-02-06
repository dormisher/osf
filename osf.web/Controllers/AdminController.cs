using System.Drawing;
using System.Web;
using System.Web.Mvc;
using ImageResizer;
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
            ValidateImage();

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

        private void ValidateImage()
        {
            HttpPostedFileBase file = Request.Files["image"];

            if (file == null ||file.ContentLength == 0)
            {
                ModelState.AddModelError("image", "Please upload an image");
            }
            else
            {
                var image = Image.FromStream(file.InputStream, true, true);
                
                if (image.Width < 620)
                {
                    ModelState.AddModelError("image", "Image must be at least 620px wide, for best results upload an image that is 620x300 or some of the image will be cut off");
                }
                else if (image.Height < 300)
                {
                    ModelState.AddModelError("image", "Image must be at least 300px high, for best results upload an image that is 620x300 or some of the image will be cut off");
                }
            }
        }
    }
}
