using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheVoid.Data;

namespace TheVoid.Controllers
{
    [Authorize]
    public class VoidController : Controller
    {
        private VoidDbContext Database;
        public VoidController(VoidDbContext voidDb)
        {
            Database = voidDb;
        }

        public IActionResult WriteToVoid()
        {
            return View();
        }

        public IActionResult ReadFromVoid()
        {
            return View();
        }

        public IActionResult VoidInteractions()
        {
            return View();
        }
    }
}
