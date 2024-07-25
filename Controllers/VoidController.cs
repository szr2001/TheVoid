using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TheVoid.Data;
using TheVoid.Interfaces;
using TheVoid.Models;
using TheVoid.ViewModels;

namespace TheVoid.Controllers
{
    [Authorize]
    public class VoidController : Controller
    {
        private readonly VoidDbContext _voidDb;
        private readonly UserManager<VoidUser> _voidUserManager;
        private readonly IConfiguration _configuration;
        private readonly IAdsHandler _adds;

        private readonly TimeSpan WriteGlobalDelay;
        private readonly TimeSpan ReadGlobalDelay;

        public VoidController(VoidDbContext voidDb, UserManager<VoidUser> usermanager, IConfiguration configuration, IAdsHandler adds)
        {
            _voidDb = voidDb;
            _voidUserManager = usermanager;
            _configuration = configuration;
            _adds = adds;

            WriteGlobalDelay = TimeSpan.FromMinutes(_configuration.GetValue<int>("WriteMinutesInterval"));
            ReadGlobalDelay = TimeSpan.FromMinutes(_configuration.GetValue<int>("ReadMinutesInterval"));
        }

        public async Task<IActionResult> WriteToVoid()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var userData = await _voidUserManager.FindByIdAsync(userId);

            if (userData == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            if ((DateTime.UtcNow - userData.LastWroteToVoid) < WriteGlobalDelay)
            {
                return RedirectToAction(nameof(VoidInteractions));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WriteToVoid(VoidMessageVM message)
        {
            //check for message lenght 

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var userData = await _voidUserManager.FindByIdAsync(userId);

            if (userData == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            if ((DateTime.UtcNow - userData.LastWroteToVoid) < WriteGlobalDelay)
            {
                return RedirectToAction(nameof(VoidInteractions));
            }

            userData.LastWroteToVoid = DateTime.UtcNow;
            userData.AddedVoidMessages++;

            await _voidUserManager.UpdateAsync(userData);

            _voidDb.VoidMessages.Add
                (
                    new VoidMessage 
                    {
                        Content = message.VoidMessage,
                        Timestamp = DateTime.UtcNow,
                    }
                );

            await _voidDb.SaveChangesAsync();

            return RedirectToAction(nameof(VoidInteractions));
        }

        public async Task<IActionResult> ReadFromVoid()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var userData = await _voidUserManager.FindByIdAsync(userId);

            if (userData == null)
            {
                return RedirectToAction("Logout", "Account");
            }

            if ((DateTime.UtcNow - userData.LastReadFromVoid) < ReadGlobalDelay)
            {
                return RedirectToAction(nameof(VoidInteractions));
            }

            if(_adds.CanReceieveRandomAd)
            {
                //if(if there are adds loaded)
                return RedirectToAction(nameof(AdsView));
            }

            if (!_voidDb.VoidMessages.Any())
            {
                return RedirectToAction(nameof(NoMessagesFound));
            }

            VoidMessage message = _voidDb.VoidMessages.First();

            _voidDb.VoidMessages.Remove(message);

            await _voidDb.SaveChangesAsync();

            userData.LastReadFromVoid = DateTime.UtcNow;
            userData.RetrivedVoidMessages++;

            await _voidUserManager.UpdateAsync(userData);

            VoidMessageVM voidMessage = new()
            {
                VoidMessage = message.Content
            };
            
            return View(voidMessage);
        }

        public IActionResult NoMessagesFound()
        {
            return View();
        }

        public IActionResult AdsView()
        {
            //load add and pass it to the view
            AdsVM addsVM = new AdsVM();
            return View(addsVM);
        }

        public async Task<IActionResult> VoidInteractions()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var userData = await _voidUserManager.FindByIdAsync(userId);

            if (userData == null)
            {
                return RedirectToAction("Logout", "Account");
            }
            
            DateTime UserLastWrite = userData.LastWroteToVoid;
            DateTime UserLastRead = userData.LastReadFromVoid;

            TimeSpan writeDelay = CalculateRemainingTime(UserLastWrite, WriteGlobalDelay);
            TimeSpan readDelay = CalculateRemainingTime(UserLastRead, ReadGlobalDelay);

            int totalRead = userData.RetrivedVoidMessages;
            int totalWrite = userData.AddedVoidMessages;
            bool canWrite = writeDelay == TimeSpan.Zero ? true : false;
            bool canRead = readDelay == TimeSpan.Zero ? true : false;

            VoidInteractionsVM voidInteractionsVM = new()
            {
                CanWrite = canWrite,
                CanRead = canRead,
                TotalMessagesRead = totalRead,
                TotalMessagesWrite = totalWrite,
                WriteDelay = writeDelay,
                ReadDelay = readDelay
            };

            return View(voidInteractionsVM);
        }

        private TimeSpan CalculateRemainingTime(DateTime lastInterval, TimeSpan delay)
        {
            TimeSpan remainingTime = delay - (DateTime.UtcNow - lastInterval);
            return remainingTime > TimeSpan.Zero ? remainingTime : TimeSpan.Zero;
        }
    }
}
