
using TheVoid.Enums;

namespace TheVoid.Models.Items
{
    public class ItemBase
    {
        public int Name { get; set; }
        public int Description { get; set; }
        public string? IconPath { get; set; }
        public ItemType Type { get; set; }
    }
}
