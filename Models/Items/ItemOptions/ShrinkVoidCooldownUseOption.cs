using Microsoft.AspNetCore.Identity;
using TheVoid.Data;
using TheVoid.Models.Items.Clases;

namespace TheVoid.Models.Items.Functionalities
{
    public class ShrinkVoidCooldownUseOption : ItemOptionBase
    {
        private readonly VoidDbContext _voidDb;
        private readonly UserManager<VoidUser> _voidUserManager;

        public ShrinkVoidCooldownUseOption(VoidDbContext voidDb, UserManager<VoidUser> voidUserManager)
        {
            _voidDb = voidDb;
            _voidUserManager = voidUserManager;
        }

        public override void ExecuteFunctionality()
        {
            throw new NotImplementedException();
        }

    }
}
