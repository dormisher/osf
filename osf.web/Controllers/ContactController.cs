using System.Web.Mvc;
using osf.web.Services;

namespace osf.web.Controllers
{
	public class ContactController : Controller
	{
		MailService _mailService = new MailService();

		[HttpPost]
		public void SendFeedback(string email, string message)
		{
			_mailService.SendFeedbackMail(email, message);
		}
	}
}