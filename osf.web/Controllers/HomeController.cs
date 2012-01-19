using System.Web.Mvc;
using osf.web.Models;

namespace osf.web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
        	var model = new HomeModel();

            return View();
        }
	}
}
