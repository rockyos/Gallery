using System.ComponentModel.DataAnnotations;

namespace CoreTest.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Enter your e-mail or login")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
