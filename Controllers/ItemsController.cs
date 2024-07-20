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
        private readonly ItemsHandler _itemHandler;

        public ItemsController(VoidDbContext voidDb, UserManager<VoidUser> voidUserManager, ItemsHandler itemHandler)
        {
            _voidDb = voidDb;
            _voidUserManager = voidUserManager;
            _itemHandler = itemHandler;
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
                items.Add(new(itemdata.Type, _itemHandler.ItemPrefabs[itemdata.Type].IconPath));
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
            ItemBase itemprefab = _itemHandler.ItemPrefabs[type];

            ItemDataVM selecteditem = new(itemprefab.Type, itemprefab.Name, itemprefab.Description, itemprefab.IconPath, itemprefab.Rarity ,itemprefab.Options.Keys.ToArray());

            List<ItemData> playerItems = _voidDb.Items.Where(i => i.UserId == userData.Id).ToList();
            List<ItemVM> items = new();

            foreach (var itemdata in playerItems)
            {
                items.Add(new(itemdata.Type, _itemHandler.ItemPrefabs[itemdata.Type].IconPath));
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

            ItemData? specifiedItem = _voidDb.Items.Where(i => i.User == userData && i.Type == item).FirstOrDefault();
            
            if (specifiedItem != null)
            {
                Console.WriteLine($"Option '{option}' Triggered for item '{item}' in invetory of user with email {userData.Email}");
                await _itemHandler.ExecuteItemOption(User, item, option);
            }

            return RedirectToAction(nameof(Inventory));
        }
    }
}
