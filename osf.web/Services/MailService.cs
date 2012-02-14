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
				//smtpClient.Send(msg);
			}
		}

		//internal void ValidateGrantApplication(GrantApplicationModel grantApplication, ModelStateDictionary modelState, HttpPostedFileBase file)
		//{
		//    ValidateContactModel(grantApplication.Contact1, modelState);
		//    ValidateContactModel(grantApplication.Contact2, modelState);

		//    if (string.IsNullOrEmpty(grantApplication.InsuranceNumber))
		//    {
		//        modelState.AddModelError("InsuranceNumber", "please enter your insurance number");
		//    }

		//    if (file.ContentLength == 0)
		//    {
		//        modelState.AddModelError("child", "please upload child protection policy");
		//    }
		//}

		//private void ValidateContactModel(ContactModel contactModel, ModelStateDictionary modelState)
		//{
		//    if (string.IsNullOrEmpty(contactModel.Name))
		//    {
		//        modelState.AddModelError("Name", "please enter your name");
		//    }

		//    if (string.IsNullOrEmpty(contactModel.Email))
		//    {
		//        modelState.AddModelError("Email", "please enter your email address");
		//    }

		//    if (string.IsNullOrEmpty(contactModel.Address))
		//    {
		//        modelState.AddModelError("Address", "please enter your address");
		//    }

		//    if (string.IsNullOrEmpty(contactModel.Position))
		//    {
		//        modelState.AddModelError("Email", "please enter your position");
		//    }

		//    if (string.IsNullOrEmpty(contactModel.Telephone))
		//    {
		//        modelState.AddModelError("Email", "please enter your phone number");
		//    }
		//}
	}
}