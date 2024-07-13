using TheVoid.Enums;

namespace TheVoid.ViewModels
{
    public class ItemDataVM
    {
        public ItemType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconPath { get; set; }
        public ItemRarity Rarity { get; set; }
        public ItemFunctionalityType[] Options { get; set; }

        public ItemDataVM(ItemType type, string name, string description, string iconPath, ItemRarity rarity, ItemFunctionalityType[] options)
        {
            Type = type;
            Name = name;
            Description = description;
            IconPath = iconPath;
            Options = options;
            Rarity = rarity;
        }
    }
}
