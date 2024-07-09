﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TheVoid.Data;
using TheVoid.Enums;
using TheVoid.Models;
using TheVoid.Models.Items.Clases;
using TheVoid.Models.Items.Functionalities;
using TheVoid.ViewModels;

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
                        "Assets/VoidShardThumb.png",
                        ItemType.VoidShard,
                        new()
                        {
                            {ItemFunctionalityType.Use, new ShrinkVoidCooldownUseFunc(_voidDb, _voidUserManager) },
                            {ItemFunctionalityType.Delete, new DeleteFunc(_voidDb, _voidUserManager) },
                        }
                    )},
                    {ItemType.VoidPermit, new VoidPermit
                    (
                        "Void Permit",
                        "Permit offered by the Void Organization for access to Void Energy, valid until the year 87963.",
                        "Assets/VoidPermitThumb.png",
                        ItemType.VoidPermit,
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
                return RedirectToAction("VoidInteractions","Void");
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
    }
}