﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication12.Models;
using WebApplication12.ViewModal;

namespace WebApplication12.Controllers
{

    public class Adminstration : Controller
    {

        #region ---------- GLobal variable ----------

       
        private readonly RoleManager<IdentityRole> Rolemanager;
        private readonly UserManager<ApplicationUser> userManager;

        #endregion


        #region --------- CONSTRUCTOR ---------      

        public Adminstration(RoleManager<IdentityRole> _Rolemanager, UserManager<ApplicationUser> _userManager)
        {
            this.Rolemanager = _Rolemanager;
            this.userManager = _userManager;
        }
        #endregion 

         
        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with {id} cannot be found";
                return View("NotFound");
            }

            var userClaims = await userManager.GetClaimsAsync(user);
            var userRoles = await userManager.GetRolesAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                City = user.City,
                UserName = user.UserName,
                Roles = userRoles,
                Claims = userClaims.Select(q => q.Value).ToList()
            };

            return View(model);

        
                
        }


        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id {id} cannot be found";
                return View("NotFound");
            }

            else
            {
                var Result = await userManager.DeleteAsync(user);
                if (Result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in Result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View("ListUsers");
            }

        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            var user = await Rolemanager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id {id} cannot be found";
                return View("NotFound");
            }

            else
            {
                try
                {
                    var Result = await Rolemanager.DeleteAsync(user);
                    if (Result.Succeeded)
                    {
                        return RedirectToAction("LIstRole");
                    }

                    foreach (var error in Result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (System.Exception)
                {
                    ViewBag.ErrorTitle = $"{user.Name} role is in use";
                    ViewBag.ErrorMessage = $"{user.Name} role cannot be deleted ";

                    return View("Error");

                }

                return View("LIstRole");
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel modal)
        {
            var user = await userManager.FindByIdAsync(modal.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with {modal.UserName} cannot be found";
                return View("NotFound");
            }
            else
            {

                user.UserName = modal.UserName;
                user.City = modal.City;
                user.Email = modal.Email;


                var reuslt = await userManager.UpdateAsync(user);

                if (reuslt.Succeeded)
                {
                    return RedirectToAction("ListUsers");

                }


                foreach (var error in reuslt.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                
                    
            }
            return View(modal);



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
                    return RedirectToAction("LIstRole", "Adminstration");
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
        [HttpGet]
        public async Task<IActionResult> EditRole(string id, string name)
        {
            var role = await Rolemanager.FindByIdAsync(id);

            if (role is null)
            {
                ViewBag.ErrorMessage = $"Role {name } is not found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {

                 if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    model.User.Add(user.UserName);
                }
            }

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await Rolemanager.FindByIdAsync(model.id);

            if (role is null)
            {
                ViewBag.ErrorMessage = $"Role {model.RoleName } is not found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                 var result = await  Rolemanager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("LIstRole");
                }


                foreach (var erroe in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, erroe.Description);
                }
                return View(model);
            }  
                    

           

        }


        public async Task<IActionResult> EditUserInRole(string roleId)
        {
            ViewBag.roleid = roleId;

            var role = await Rolemanager.FindByIdAsync(roleId);
            if (role == null)
            {

                ViewBag.ErrorMessage = $"role with id {roleId} cannot be found";
                return View("Not Found");
            }

            else
            {
                var model = new List<UserRoleViewModel>();
                foreach (var item in userManager.Users)
                {
                    var userRoleViewModel = new UserRoleViewModel
                    {
                        UserId = item.Id,
                        UserName = item.UserName,

                    };


                    if (await userManager.IsInRoleAsync(item , role.Name))
                    {
                        userRoleViewModel.IsSelected = true;
                    }

                    else
                    {
                        userRoleViewModel.IsSelected = false;
                    }
                     
                    model.Add(userRoleViewModel);
                }


                 return View(model);
            }

        }

        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model , string roleId)
        {
            var role = await Rolemanager.FindByIdAsync(roleId);
            if (role == null)
            {

                ViewBag.ErrorMessage = $"role with id {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                 var Users = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                //yedi user select chaina vne 

                if (model[i].IsSelected &&   !(await userManager.IsInRoleAsync(Users, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(Users, role.Name);

                }

                //user lai vako role hatauna lai

                else if (!model[i].IsSelected && (await userManager.IsInRoleAsync(Users, role.Name)))
                {
                    result = await userManager.RemoveFromRoleAsync(Users, role.Name);
                }

                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < model.Count -1)
                    {
                        continue;
                    }

                    else
                    {
                        return RedirectToAction("EditRole", new { Id = roleId });
                    }
                }


            }

            return RedirectToAction("EditRole", new { Id = roleId });

        }










    }
}
