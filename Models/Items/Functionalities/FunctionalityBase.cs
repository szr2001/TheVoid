using TheVoid.Enums;

namespace TheVoid.Models.Items.Functionalities
{
    public abstract class FunctionalityBase
    {
        public ItemFunctionalityType Type { get; set; }
        public abstract void ExecuteFunctionality();
    }
}
