using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebApplication12.Models;
using WebApplication12.ViewModal;

namespace WebApplication12.Controllers
{

    public class Adminstration : Controller
    {

        #region ---------- GLobal variable ----------

       
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly UserManager<ApplicationUser> _userManager;

        #endregion


        #region --------- CONSTRUCTOR ---------      

        public Adminstration(RoleManager<IdentityRole> rolemanager, UserManager<ApplicationUser> userManager)
        {
            this._rolemanager = rolemanager;
            this._userManager = userManager;
        }
        #endregion

        #region --------- USERS---------

      

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with {id} cannot be found";
                return View("NotFound");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

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
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with id {id} cannot be found";
                return View("NotFound");
            }

            else
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View("ListUsers");
            }

        }
        #endregion
        [HttpPost]
     
        public async Task<IActionResult> DeleteRole(string id)
        {
            var user = await _rolemanager.FindByIdAsync(id);
            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with id {id} cannot be found";
                return View("NotFound");
            }

            else
            {
                try
                {
                    var result = await _rolemanager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("LIstRole");
                    }

                    foreach (var error in result.Errors)
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

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel modal)
        {
            var user = await _userManager.FindByIdAsync(modal.Id);

            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with {modal.UserName} cannot be found";
                return View("NotFound");
            }
            else
            {

                user.UserName = modal.UserName;
                user.City = modal.City;
                user.Email = modal.Email;


                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListUsers");

                }


                foreach (var error in result.Errors)
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
            if (!ModelState.IsValid) return View(model);
            var identityRole = new IdentityRole
            {
                Name = model.RoleName
            };

            var result = await  _rolemanager.CreateAsync(identityRole);
            if (result.Succeeded)
            {
                return RedirectToAction("LIstRole", "Adminstration");
            }
            foreach (var error in result.Errors)
            {

                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult LIstRole()
        {
            var roles = _rolemanager.Roles;
            return View(roles);

        }
        [HttpGet]

        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _rolemanager.FindByIdAsync(id);

            if (role is null)
            {
                ViewBag.ErrorMessage = $"Role {id} is not found";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in _userManager.Users)
            {

                 if(await _userManager.IsInRoleAsync(user, role.Name))
                 {
                     model.User.Add(user.UserName);
                 }
            }

            return View(model);

        }

        [HttpPost]
 
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _rolemanager.FindByIdAsync(model.id);

            if (role is null)
            {
                ViewBag.ErrorMessage = $"Role {model.RoleName } is not found";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                 var result = await  _rolemanager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("LIstRole");
                }


                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }  
                    

           

        }

        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string userid)
        {
            ViewBag.roleid = userid;

            var role = await _rolemanager.FindByIdAsync(userid);
            if (role is null)
            {

                ViewBag.ErrorMessage = $"role with id {userid} cannot be found";
                return View("NotFound");
            }

            else
            {
                var model = new List<UserRoleViewModel>();
                foreach (var item in _userManager.Users)
                {
                    var userRoleViewModel = new UserRoleViewModel
                    {
                        UserId = item.Id,
                        UserName = item.UserName,

                    };


                    if (await _userManager.IsInRoleAsync(item , role.Name))
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
            var role = await _rolemanager.FindByIdAsync(roleId);
            if (role is null)
            {
                ViewBag.ErrorMessage = $"role with id {roleId} cannot be found";
                return View("NotFound");
            }

            for (var i = 0; i < model.Count; i++)
            {
                 var users = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                //yedi user select chaina vne 

                if (model[i].IsSelected &&   !(await _userManager.IsInRoleAsync(users, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(users, role.Name);

                }

                //user lai vako role hatauna lai

                else if (!model[i].IsSelected && (await _userManager.IsInRoleAsync(users, role.Name)))
                {
                    result = await _userManager.RemoveFromRoleAsync(users, role.Name);
                }

                else
                {
                    continue;
                }

                if (!result.Succeeded) continue;
                if (i < model.Count -1)
                {
                    continue;
                }

                else
                {
                    return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });

        }

       

        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }

            // UserManager service GetClaimsAsync method gets all the current claims of the user
            var existingUserClaims = await _userManager.GetClaimsAsync(user);

            var model = new UserClaimsViewModel
            {
                UserId = userId
            };

            // Loop through each claim we have in our application
            foreach (var claim in ClaimsStore.AllClaims)
            {
                var userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                // If the user has the claim, set IsSelected property to true, so the checkbox
                // next to the claim is checked on the UI
                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }

                model.Cliams.Add(userClaim);
            }

            return View (model);

        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return View("NotFound");
            }

            // Get all the user existing claims and delete them
            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "CANNOT REMOVE USER EXISTING CLAIMS");
                return View(model);
            }

            // Add all the claims that are selected on the UI
            result = await _userManager.AddClaimsAsync(user,
                model.Cliams.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimType)));

            if (result.Succeeded) return RedirectToAction("EditUser", new {Id = model.UserId});
            ModelState.AddModelError(string.Empty, "CANNOT ADD SELECTED CLAIMS TO USER");
            return View(model);

        }









    }
}
