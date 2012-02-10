namespace osf.web.Models
{
	public class GrantApplicationModel
	{
		public ContactModel Contact1 { get; set; }

		public ContactModel Contact2 { get; set; }

		public string InsuranceNumber { get; set; }

		public string VAT { get; set; }
	}

	public class ContactModel
	{
		public string Name { get; set; }

		public string Address { get; set; }

		public string Email { get; set; }

		public string Telephone { get; set; }

		public string Position { get; set; }
	}
}