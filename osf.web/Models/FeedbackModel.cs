using System.ComponentModel.DataAnnotations;

namespace osf.web.Models
{
	public class FeedbackModel
	{
		[Required]
		public string Email { get; set; }

		[Required]
		public string Message { get; set; }

		[Required]
		public string Name { get; set; }
	}
}