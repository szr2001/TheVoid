using TheVoid.Enums;
using TheVoid.Models.Items.Clases;

namespace TheVoid.Models.Items.Functionalities
{
    public abstract class ItemOptionBase
    {
        public ItemBase? Parent { get; set; }
        public ItemOptionType Type { get; set; }

        public abstract void ExecuteFunctionality();
    }
}
