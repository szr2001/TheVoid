using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TheVoid.Data;
using TheVoid.Models;
using TheVoid.ViewModels;

namespace TheVoid.Controllers
{
    [Authorize]
    public class VoidController : Controller
    {
        private VoidDbContext _voidDb;
        private UserManager<VoidUser> _voidUserManager;
        public VoidController(VoidDbContext voidDb, UserManager<VoidUser> usermanager)
        {
            _voidDb = voidDb;
            _voidUserManager = usermanager;
        }

        public IActionResult WriteToVoid()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WriteToVoid(VoidMessageVM message)
        {
            Console.WriteLine($"Message {User.Identity!.Name} send  {message.VoidMessage}");

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if(userId == null)
            {
                ModelState.AddModelError("", "UserID not found");
                return View(message);
            }

            var userData = await _voidUserManager.FindByIdAsync(userId);

            if (userData == null)
            {
                ModelState.AddModelError("", "UserData not found");
                return View(message);
            }

            userData.LastWroteToVoid = DateTime.Now;

            Console.WriteLine($"userId {userId}, Userdata Id {userData.Id}");
            Console.WriteLine($"Last wrote to void {userData.LastWroteToVoid}");
            // Save changes to database
            await _voidUserManager.UpdateAsync(userData);

            _voidDb.VoidMessages.Add
                (
                    new VoidMessage 
                    {
                        Content = message.VoidMessage,
                        Timestamp = DateTime.Now,
                    }
                );

            await _voidDb.SaveChangesAsync();

            return RedirectToAction(nameof(VoidInteractions));
        }

        public IActionResult ReadFromVoid()
        {
            return View();
        }

        public IActionResult RetrieveFromVoid()
        {
            VoidMessage voidMessage = new VoidMessage();
            return View(voidMessage);
        }

        public IActionResult NoMessagesFound()
        {
            return View();
        }

        public IActionResult VoidInteractions()
        {
            return View();
        }
    }
}
