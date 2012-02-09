using System.Web.Mvc;
using osf.web.Services;

namespace osf.web.Controllers
{
    public class EventsController : Controller
    {
		private EventService _eventService = new EventService();

        public ActionResult Index(int page = 1)
        {
            return View(_eventService.LoadPagedEvents(page, 4));
        }
    }
}
