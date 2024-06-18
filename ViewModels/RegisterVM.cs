using System.ComponentModel.DataAnnotations;

namespace TheVoid.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Email is required.")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage ="Passwords don't match.")]
        public string? ConfirmPassword { get; set; }
    }
}
