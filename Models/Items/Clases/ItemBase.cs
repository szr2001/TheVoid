using TheVoid.Enums;
using TheVoid.Models.Items.Functionalities;

namespace TheVoid.Models.Items.Clases
{
    public abstract class ItemBase
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? IconPath { get; set; }
        public ItemType Type { get; set; }
        public ItemRarity Rarity { get; set; }
        public Dictionary<ItemFunctionalityType, FunctionalityBase> Options { get; set; } = new();

        public ItemBase(string name, string description, string? iconPath, ItemType type, Dictionary<ItemFunctionalityType, FunctionalityBase> options, ItemRarity rarity)
        {
            Name = name;
            Description = description;
            IconPath = iconPath;
            Type = type;
            Options = options;
            Rarity = rarity;

            foreach (var function in Options)
            {
                function.Value.Parent = this;
            }
        }
    }
}
