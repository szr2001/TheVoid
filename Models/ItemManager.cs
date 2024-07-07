using TheVoid.Enums;
using TheVoid.Models.Items;

namespace TheVoid.Models
{
    public class ItemManager
    {
        public readonly Dictionary<ItemType, ItemBase> ItemPrefabs = new()
        {
            {ItemType.VoidShard, new VoidShard() }
        };

        public ItemBase Getitem(ItemType type)
        {
            return ItemPrefabs[type];
        }
    }
}
