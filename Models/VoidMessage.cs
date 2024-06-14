using System.ComponentModel.DataAnnotations;

namespace TheVoid.Models
{
    public class VoidMessage
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="There must be a message")]
        public string? Content { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
