using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using TheVoid.Data;
using TheVoid.Enums;
using TheVoid.Models;
using TheVoid.Models.Items.Clases;
using TheVoid.Models.Items.Functionalities;
using TheVoid.ViewModels.Items;

namespace TheVoid.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly VoidDbContext _voidDb;
        private readonly UserManager<VoidUser> _voidUserManager;

        private Dictionary<ItemType, ItemBase> ItemPrefabs = [];

        public ItemsController(VoidDbContext voidDb, UserManager<VoidUser> voidUserManager)
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
                        "Lowers the Write/Read to void delay by 5 minutes on use.",
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

        public async Task<IActionResult> Inventory()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var userData = await _voidUserManager.FindByIdAsync(userId);

            if (userData == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            List<ItemData> playerItems = _voidDb.Items.Where(i => i.UserId == userData.Id).ToList();
            List<ItemVM> items = new();

            foreach (var itemdata in playerItems) 
            {
                items.Add(new(itemdata.Type, ItemPrefabs[itemdata.Type].IconPath));
            }

            InventoryVM inventoryData = new(items);
            return View(inventoryData);
        }

        [HttpPost]
        public async Task<IActionResult> Inventory(ItemType type)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var userData = await _voidUserManager.FindByIdAsync(userId);

            if (userData == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            ItemData? selectedItemData = await _voidDb.Items.Where(i => i.UserId == userData.Id).Where(i => i.Type == type).FirstOrDefaultAsync();
            if(selectedItemData == null)
            {
                return RedirectToAction("Profile", "Account");
            }
            ItemBase itemprefab = ItemPrefabs[type];

            ItemDataVM selecteditem = new(itemprefab.Type, itemprefab.Name, itemprefab.Description, itemprefab.IconPath, itemprefab.Rarity ,itemprefab.Options.Keys.ToArray());

            List<ItemData> playerItems = _voidDb.Items.Where(i => i.UserId == userData.Id).ToList();
            List<ItemVM> items = new();

            foreach (var itemdata in playerItems)
            {
                items.Add(new(itemdata.Type, ItemPrefabs[itemdata.Type].IconPath));
            }

            InventoryVM inventoryData = new(items);
            inventoryData.SelectedItem = selecteditem;

            return View(inventoryData);
        }

        [HttpPost]
        public async Task<IActionResult> TriggerItemOption(ItemType item, ItemOptionType option)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var userData = await _voidUserManager.FindByIdAsync(userId);

            if (userData == null)
            {
                return RedirectToAction("VoidInteractions", "Void");
            }

            //pass the itemID from the db to users and check for specific ID and type to have a better
            //acuracy on using a specific item instead of a item type for items that might have specific data asigned to it
            ItemData? specifiedItem = userData.Items.Where(i => i.Type == item).FirstOrDefault();
            
            if (specifiedItem != null)
            {
                Console.WriteLine($"Option '{option}' Triggered in '{item}'");
            }

            return RedirectToAction(nameof(Inventory));
        }
    }
}
