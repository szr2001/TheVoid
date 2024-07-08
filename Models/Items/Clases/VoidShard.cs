using TheVoid.Enums;
using TheVoid.Models.Items.Functionalities;

namespace TheVoid.Models.Items.Clases
{
    public class VoidShard : ItemBase
    {
        public VoidShard(string name, string description, string? iconPath, ItemType type, Dictionary<ItemFunctionalityType, FunctionalityBase> options) : base(name, description, iconPath, type, options)
        {
        }
    }
}
