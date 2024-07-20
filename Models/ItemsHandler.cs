using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TheVoid.Data;
using TheVoid.Enums;
using TheVoid.Models.Items.Clases;
using TheVoid.Models.Items.Functionalities;

namespace TheVoid.Models
{
    public class ItemsHandler
    {
        private readonly VoidDbContext _voidDb;
        private readonly UserManager<VoidUser> _voidUserManager;

        public Dictionary<ItemType, ItemBase> ItemPrefabs = [];

        public ItemsHandler(VoidDbContext voidDb, UserManager<VoidUser> voidUserManager)
        {
            _voidDb = voidDb;
            _voidUserManager = voidUserManager;
            GenerateItemPrefabs();
        }

        private void GenerateItemPrefabs()
        {
            ItemPrefabs = new()
            {
                {ItemType.VoidShard, new VoidShard
                    (
                        "Void Shard",
                        "Removes the cooldowns on Void Interactions when used.",
                        "../Assets/Images/ItemIcons/VoidShard.png",
                        ItemType.VoidShard,
                        ItemRarity.Common,
                        new()
                        {
                            {ItemOptionType.Use, new ShrinkVoidCooldownUseOption(_voidDb, _voidUserManager) },
                            {ItemOptionType.Delete, new DeleteOption(_voidDb, _voidUserManager) },
                        }
                    )},
                    {ItemType.VoidPermit, new VoidPermit
                    (
                        "Void Permit",
                        "Permit offered by the Void Organization for access to Void Energy, valid until the year 87963.",
                        "../Assets/Images/ItemIcons/VoidPermit.png",
                        ItemType.VoidPermit,
                        ItemRarity.Common,
                        new()
                        {
                        }
                    )}
            };
        }

        public async Task ExecuteItemOption(ClaimsPrincipal User, ItemType item, ItemOptionType option)
        {
            if (!ItemPrefabs.ContainsKey(item)) return;
            if (!ItemPrefabs[item].Options.TryGetValue(option, out ItemOptionBase? Option)) return;

            await Option.ExecuteFunctionality(User);
        }

        public async Task TriggerRandomItemDrop()
        {
        }

        public async Task TriggerItemDrop(ItemType type)
        {
        }
    }
}
