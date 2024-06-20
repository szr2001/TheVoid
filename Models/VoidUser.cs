using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TheVoid.Models
{
    public class VoidUser:IdentityUser
    {
        [MaxLength(100)]
        [StringLength(100)]
        [Required]
        public string? Name { get; set; }
        public DateTime LastWroteToVoid { get; set; }
        public DateTime LastReadFromVoid { get; set; }
        public int RetrivedVoidMessages { get; set; }
        public int AddedVoidMessages { get; set; }
    }
}
