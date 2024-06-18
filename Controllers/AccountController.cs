using Microsoft.AspNetCore.Mvc;
using TheVoid.Data;

namespace TheVoid.Controllers
{
    public class AccountController : Controller
    {
        private VoidDbContext Database;
        public AccountController(VoidDbContext voidDb) 
        {
            Database = voidDb;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
