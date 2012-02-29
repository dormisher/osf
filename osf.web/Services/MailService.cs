using System.Net.Mail;

namespace osf.web.Services
{
	public class MailService
	{
		internal void SendFeedbackMail(string email, string message)
		{
			var msg = new MailMessage();
			msg.From = new MailAddress(email);
			msg.Subject = "Feedback Submitted From opportunitysports.org";
			msg.Body = message;
			msg.To.Add(new MailAddress("info@opportunitysports.org"));

			using (var smtpClient = new SmtpClient())
			{
				smtpClient.Send(msg);
			}
		}
	}
}