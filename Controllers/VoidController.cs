using Microsoft.AspNetCore.Mvc;
using TheVoid.Data;

namespace TheVoid.Controllers
{
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
    }
}
