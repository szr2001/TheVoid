using TheVoid.Enums;

namespace TheVoid.ViewModels
{
    public class ItemDataVM
    {
        public ItemType Type { get; set; }
        public ItemFunctionalityType[]? Options { get; set; }

        public ItemDataVM(ItemType type, ItemFunctionalityType[]? options)
        {
            Type = type;
            Options = options;
        }
    }
}
