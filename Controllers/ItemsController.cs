using AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheVoid.Data;
using TheVoid.Enums;
using TheVoid.Models;
using TheVoid.Models.Items.Clases;
using TheVoid.Models.Items.Functionalities;

namespace TheVoid.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly VoidDbContext _voidDb;
        private readonly UserManager<VoidUser> _voidUserManager;

        private Dictionary<ItemType, ItemBase> ItemPrefabs = new();

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
                        "Lowers the Write/Read to void delay by 5 minutes on use",
                        "Assets/VoidShardThumb",
                        ItemType.VoidShard,
                        new()
                        {
                            {ItemFunctionalityType.Use, new ShrinkVoidCooldownUseFunct(_voidDb, _voidUserManager) },
                            {ItemFunctionalityType.Delete, new DeleteFunc(_voidDb, _voidUserManager) },
                        }
                    )}
            };
        }

        public ItemBase Getitem(ItemType type)
        {
            return ItemPrefabs[type];
        }
    }
}
