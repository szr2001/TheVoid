using System.ComponentModel.DataAnnotations;

namespace TheVoid.ViewModels
{
    public class VoidMessageVM
    {
        [MaxLength(500, ErrorMessage = "Message is too long")]
        [MinLength(50, ErrorMessage ="Message is too short")]
        [Required(ErrorMessage = "Message is required")]
        public string? VoidMessage {  get; set; }
    }
}
