using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication12.ViewModal;

namespace WebApplication12.Controllers
{
    public class Adminstration : Controller
    {
        public readonly RoleManager<IdentityRole> Rolemanager;

        public Adminstration(RoleManager<IdentityRole> _Rolemanager)
        {
            Rolemanager = _Rolemanager;
        }

        
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await  Rolemanager.CreateAsync(identityRole);
               if (result.Succeeded)
                {
                    return RedirectToAction("index", "home");
                }

                foreach (IdentityError error in result.Errors)
                {

                    ModelState.AddModelError(string.Empty, error.Description);
                }


            }

           
            return View(model);
        }


        [HttpGet]
        public IActionResult LIstRole()
        {
            var roles = Rolemanager.Roles;
            return View(roles);

        }
    }
}
