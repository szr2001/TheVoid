using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TheVoid.Data;

namespace TheVoid.Models.Items.Functionalities
{
    public class DeleteOption : ItemOptionBase
    {

        public DeleteOption(VoidDbContext voidDb, UserManager<VoidUser> voidUserManager) : base(voidDb, voidUserManager)
        {
        }

        public override async Task ExecuteFunctionality(ClaimsPrincipal User)
        {
            var OwningData = await TryGetItemOwiningData(User, Parent!.Type);
            if (OwningData.Item1 != null)
            {
                _voidDb.Items.Remove(OwningData.Item1);
                _voidDb.SaveChanges();
            }
        }
    }
}
