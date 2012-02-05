using System.Web.Mvc;
using osf.web.Models;
using osf.web.Services;

namespace osf.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly EventService _eventService = new EventService();

        public ActionResult Index()
        {
            var model = new HomeModel {LatestEvents = _eventService.LoadEvents(5)};

            return View(model);
        }
	}
}