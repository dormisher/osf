using System.Web.Mvc;
using osf.web.Models;
using osf.web.Services;

namespace osf.web.Controllers
{
	public class ContactController : Controller
	{
		MailService _mailService = new MailService();

		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Index(FeedbackModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			_mailService.SendFeedbackMail(model);

			ViewBag.MessageSent = true;

			return View();
		}
	}
}