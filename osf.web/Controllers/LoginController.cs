using System.Web.Mvc;
using System.Web.Security;
using osf.web.Models;

namespace osf.web.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (FormsAuthentication.Authenticate(model.Username, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.Username, true);

                return RedirectToAction("index", new { Controller = "admin" });
            }

            ModelState.AddModelError("Username", "incorrect username/password combination");

            return View(model);
        }
	}
}