using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TheVoid.Data;
using TheVoid.Enums;
using TheVoid.Models.Items.Clases;

namespace TheVoid.Models.Items.Functionalities
{
    public abstract class ItemOptionBase
    {
        public ItemBase? Parent { get; set; }
        public ItemOptionType Type { get; set; }

        protected readonly VoidDbContext _voidDb;
        protected readonly UserManager<VoidUser> _voidUserManager;

        protected ItemOptionBase(VoidDbContext voidDb, UserManager<VoidUser> voidUserManager)
        {
            _voidDb = voidDb;
            _voidUserManager = voidUserManager;
        }

        public abstract Task ExecuteFunctionality(ClaimsPrincipal User);

        protected async Task<(ItemData?, VoidUser?)> TryGetItemOwiningData(ClaimsPrincipal User, ItemType type)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var userData = await _voidUserManager.FindByIdAsync(userId);

            if (userData == null) return (null,null);

            ItemData? item = _voidDb.Items.Where(i => i.User == userData && i.Type == Parent!.Type).FirstOrDefault();
            
            if(item == null) return (null,null);

            return (item,userData);
        }  
    }
}
