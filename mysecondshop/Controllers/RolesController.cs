using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RestWebAppl.Models;
using RestWebAppl.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace RestWebAppl.Controllers
{
    //Controller for moderator . Shows views for editing and adding users roles. Deleting users
    [Authorize(Roles = "Moderator")]
    public class RolesController : Controller
    {
        RoleManager<IdentityRole> roleMngr;
        private UserManager<ApplicationUser> userManager;
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userMngr)
        {
            roleMngr=roleManager;
            userManager=userMngr;
        }

        public IActionResult Index()
        {
            return View(roleMngr.Roles.ToList());
        }
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await roleMngr.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await roleMngr.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await roleMngr.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }
        public IActionResult UserList() => View(userManager.Users.ToList());

        public async Task<IActionResult> Edit(string userId)
        {
            // получаем пользователя
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // получем список ролей пользователя
                var userRoles = await userManager.GetRolesAsync(user);
                var allRoles = roleMngr.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            // Gets user
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                // Get user roles
                var userRoles = await userManager.GetRolesAsync(user);
                // Get all roles
                var allRoles = roleMngr.Roles.ToList();
                // Get list of added roles
                var addedRoles = roles.Except(userRoles);
                // Gets list of removed roles
                var removedRoles = userRoles.Except(roles);

                await userManager.AddToRolesAsync(user, addedRoles);

                await userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }
        public async Task<IActionResult> DeleteUser(string userId, List<string> roles)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
                return RedirectToAction("UserList");
            }
            return NotFound();
        }
    }
}
