using System.ComponentModel.DataAnnotations;

namespace osf.web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "enter a username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "enter a password")]
        public string Password { get; set; }
    }
}