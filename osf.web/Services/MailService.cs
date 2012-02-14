using System.Net.Mail;

namespace osf.web.Services
{
	public class MailService
	{
		internal void SendFeedbackMail(string email, string message)
		{
			var msg = new MailMessage();
			msg.From = new MailAddress(email);
			msg.Body = string.Format("<h1>Feedback Submitted</h1><p>{0}</p>", message);
			msg.To.Add(new MailAddress("dormisher@gmail.com"));
			msg.To.Add(new MailAddress("info@opportunitysports.org"));
			msg.IsBodyHtml = true;

			using (var smtpClient = new SmtpClient())
			{
				smtpClient.Send(msg);
			}
		}
	}
}