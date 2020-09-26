using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication12.Models;
using WebApplication12.ViewModal;

namespace WebApplication12.Controllers
{
    public class AccountController : Controller
    {                                  
        public readonly SignInManager<ApplicationUser> SignInManager;
        public readonly UserManager<ApplicationUser> UserManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
        }


        [HttpGet]
     
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
       
        public async Task<IActionResult> Register(RegisterViewModal model)
        {
            if (!ModelState.IsValid) return View();
            var user = new ApplicationUser 
            {
                UserName = model.Email, 
                Email = model.Email,
                City = model.City
            };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (SignInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                {
                    return RedirectToAction("ListUsers", "Adminstration");
                }

                await SignInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            } 
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description); 
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("index", "home");

        }

        [HttpGet]
        [AllowAnonymous]

        public IActionResult Login()
        {
            ModelState.Clear();
            return View();
        }

        [HttpPost]
        
        public async Task<IActionResult> Login(LoginViewModule model, string returnUrl)
        {
            if (!ModelState.IsValid) return View();
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);

                }

                return RedirectToAction("index", "home");
            }
            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            return View();
        }

        [AcceptVerbs("GET", "POST")]
     
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            return user is null ? Json(true) : Json($" {email} is already exist");
        }

        [HttpGet]
        
        public IActionResult AccessDenied()
        {
            return View();
        }





    }
}
