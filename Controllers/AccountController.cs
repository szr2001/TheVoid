using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheVoid.Enums;
using TheVoid.Models;
using TheVoid.Models.Items.Clases;
using TheVoid.ViewModels;

namespace TheVoid.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<VoidUser> signInManager;
        private readonly UserManager<VoidUser> userManager;
        public AccountController(SignInManager<VoidUser> signinmanager, UserManager<VoidUser> usermanager)
        {
            signInManager = signinmanager;
            userManager = usermanager;
        }

        public IActionResult Login()
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("VoidInteractions", "Void");
            }
            return View();
        }

        public IActionResult WelcomeToVoid()
        {
            return View();
        }

        public IActionResult Profile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM logindata)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(logindata.Email!,logindata.Password!, logindata.RememberMe, false);
                if(result.Succeeded)
                {
                    return RedirectToAction("VoidInteractions", "Void");
                }
                else
                {
                    Console.WriteLine("Wrong Username or Password");
                    ModelState.AddModelError("", "Wrong Username or Password");
                    return View(logindata);
                }
            }
            return View(logindata);
        }

        public IActionResult Register()
        {
            if (signInManager.IsSignedIn(User))
            {
                return RedirectToAction("VoidInteractions", "Void");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerData)
        {
            if (ModelState.IsValid)
            {
                VoidUser user = new()
                {
                    Name = registerData.Email,
                    UserName = registerData.Email,
                    Email = registerData.Email,
                    LastReadFromVoid = DateTime.MinValue,
                    LastWroteToVoid = DateTime.MinValue,
                    LastPremiumPurchase = DateTime.MinValue,
                    AddedVoidMessages = 0,
                    RetrivedVoidMessages = 0,
                    Level = 1,
                    Xp = 0,
                    Banned = false
                };
                user.Items.Add(new ItemData { Type = ItemType.VoidPermit, UserId = user.Id });

                var result = await userManager.CreateAsync(user, registerData.Password!);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("VoidInteractions", "Void");
                }

                foreach(var error in result.Errors)
                {
                    Console.WriteLine(error.Description);
                    ModelState.AddModelError("",error.Description);
                }
            }
            return View(registerData);
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            
            return RedirectToAction(nameof(Login));
        }
    }
}
