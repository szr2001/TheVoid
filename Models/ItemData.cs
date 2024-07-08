    using System.ComponentModel.DataAnnotations;
    using TheVoid.Enums;

namespace TheVoid.Models
{
    public class ItemData
    {
        [Key]
        public int Id { get; set; }
        public ItemType Type { get; set; }
        public string? UserId { get; set; }
        public VoidUser? User { get; set; }
    }
}
