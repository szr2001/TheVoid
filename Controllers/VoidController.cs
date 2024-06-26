﻿using Microsoft.AspNetCore.Authorization;
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
        private readonly IAddsHandler _adds;

        private readonly TimeSpan WriteGlobalDelay;
        private readonly TimeSpan ReadGlobalDelay;

        public VoidController(VoidDbContext voidDb, UserManager<VoidUser> usermanager, IConfiguration configuration, IAddsHandler adds)
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

            if (userId == null)
            {
                ModelState.AddModelError("", "UserID not found");
                return RedirectToAction(nameof(VoidInteractions));
            }

            var userData = await _voidUserManager.FindByIdAsync(userId);

            if (userData == null)
            {
                ModelState.AddModelError("", "UserData not found");
                return RedirectToAction(nameof(VoidInteractions));
            }

            if ((DateTime.Now - userData.LastWroteToVoid) < WriteGlobalDelay)
            {
                return RedirectToAction(nameof(VoidInteractions));
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> WriteToVoid(VoidMessageVM message)
        {
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

            if ((DateTime.Now - userData.LastWroteToVoid) < WriteGlobalDelay)
            {
                return RedirectToAction(nameof(VoidInteractions));
            }

            userData.LastWroteToVoid = DateTime.Now;
            userData.AddedVoidMessages++;

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

        public async Task<IActionResult> ReadFromVoid()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (userId == null)
            {
                return RedirectToAction(nameof(VoidInteractions));
            }

            var userData = await _voidUserManager.FindByIdAsync(userId);

            if (userData == null)
            {
                ModelState.AddModelError("", "UserData not found");
                return RedirectToAction(nameof(VoidInteractions));
            }

            if ((DateTime.Now - userData.LastReadFromVoid) < ReadGlobalDelay)
            {
                return RedirectToAction(nameof(VoidInteractions));
            }

            if(_adds.CanReceieveRandomAdd)
            {
                //if(if there are adds loaded)
                return RedirectToAction(nameof(AddsView));
            }

            if (!_voidDb.VoidMessages.Any())
            {
                return RedirectToAction(nameof(NoMessagesFound));
            }

            VoidMessage message = _voidDb.VoidMessages.First();

            _voidDb.VoidMessages.Remove(message);

            await _voidDb.SaveChangesAsync();

            userData.LastReadFromVoid = DateTime.Now;
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

        public IActionResult AddsView()
        {
            //load add and pass it to the view
            AddsVM addsVM = new AddsVM();
            return View(addsVM);
        }

        public async Task<IActionResult> VoidInteractions()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            if (userId == null)
            {
                ModelState.AddModelError("", "UserID not found");
                return RedirectToAction("Logout", "Account");
            }

            var userData = await _voidUserManager.FindByIdAsync(userId);

            if (userData == null)
            {
                ModelState.AddModelError("", "UserData not found");
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
            TimeSpan remainingTime = delay - (DateTime.Now - lastInterval);
            return remainingTime > TimeSpan.Zero ? remainingTime : TimeSpan.Zero;
        }
    }
}
