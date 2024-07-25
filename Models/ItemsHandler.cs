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
        public Dictionary<ItemType, ItemBase> ItemPrefabs = [];
        
        private readonly VoidDbContext _voidDb;
        private readonly UserManager<VoidUser> _voidUserManager;
        
        private IConfiguration _configuration;
        private readonly Dictionary<ItemRarity, int> ItemDropPercentages;
        private Random rnd = new();
        public ItemsHandler(VoidDbContext voidDb, UserManager<VoidUser> voidUserManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _voidDb = voidDb;
            _voidUserManager = voidUserManager;
            
            ItemDropPercentages = new Dictionary<ItemRarity, int>
            {
                { ItemRarity.None, int.Parse(_configuration["ItemDropsPercentage:None"]!) },
                { ItemRarity.Common, int.Parse(_configuration["ItemDropsPercentage:Common"]!) },
                { ItemRarity.Uncommon, int.Parse(_configuration["ItemDropsPercentage:Uncommon"]!) },
                { ItemRarity.Rare, int.Parse(_configuration["ItemDropsPercentage:Rare"]!) },
                { ItemRarity.Epic, int.Parse(_configuration["ItemDropsPercentage:Epic"]!) },
                { ItemRarity.Legendary, int.Parse(_configuration["ItemDropsPercentage:Legendary"]!) },
                { ItemRarity.Mythic, int.Parse(_configuration["ItemDropsPercentage:Mythic"]!) }
            };
            
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
                        ItemRarity.None,
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

        public async Task<ItemType> TriggerRandomItemDrop(ClaimsPrincipal User)
        {
            int Percentage = rnd.Next(1, 100);
            ItemRarity selectedRarity = ItemDropPercentages.Where(i => i.Value !< Percentage && i.Value !> Percentage).First().Key;

            var Filtereditems = ItemPrefabs.Where(i => i.Value.Rarity == selectedRarity).ToArray();

            ItemType SelectedItem = ItemType.VoidShard;

            if(Filtereditems.Length == 0)
            {

            }
            
            return SelectedItem;
        }
    }
}
