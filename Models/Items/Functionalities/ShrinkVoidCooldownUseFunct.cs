using Microsoft.AspNetCore.Identity;
using TheVoid.Data;

namespace TheVoid.Models.Items.Functionalities
{
    public class ShrinkVoidCooldownUseFunct : FunctionalityBase
    {
        private readonly VoidDbContext _voidDb;
        private readonly UserManager<VoidUser> _voidUserManager;

        public ShrinkVoidCooldownUseFunct(VoidDbContext voidDb, UserManager<VoidUser> voidUserManager)
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
