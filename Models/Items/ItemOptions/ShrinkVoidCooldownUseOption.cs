using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TheVoid.Data;
using TheVoid.Enums;

namespace TheVoid.Models.Items.Functionalities
{
    public class ShrinkVoidCooldownUseOption : ItemOptionBase
    {
        public ShrinkVoidCooldownUseOption(VoidDbContext voidDb, UserManager<VoidUser> voidUserManager) : base(voidDb,voidUserManager)
        {
        }

        public override async Task ExecuteFunctionality(ClaimsPrincipal User)
        {
            var OwningData = await TryGetItemOwiningData(User, Parent!.Type);

            if (OwningData.Item1 != null)
            {
                OwningData.Item2!.LastReadFromVoid = DateTime.MinValue;
                OwningData.Item2!.LastWroteToVoid = DateTime.MinValue;
                await _voidUserManager.UpdateAsync(OwningData.Item2);
                ItemOptionBase? delete = Parent.Options[ItemOptionType.Delete];
                delete?.ExecuteFunctionality(User);
            }
        }
    }
}
