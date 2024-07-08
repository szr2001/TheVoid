using TheVoid.Enums;

namespace TheVoid.ViewModels
{
    public class ItemVM
    {
        public ItemType Type { get; set; }
        public ItemFunctionalityType[]? Options { get; set; }

        public ItemVM(ItemType type, ItemFunctionalityType[]? options)
        {
            Type = type;
            Options = options;
        }
    }
}
