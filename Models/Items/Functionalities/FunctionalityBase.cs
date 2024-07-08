using TheVoid.Enums;
using TheVoid.Models.Items.Clases;

namespace TheVoid.Models.Items.Functionalities
{
    public abstract class FunctionalityBase
    {
        public ItemBase? Parent { get; set; }
        public ItemFunctionalityType Type { get; set; }

        public abstract void ExecuteFunctionality();
    }
}
