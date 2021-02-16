using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballClubApplication.Controllers
{
    [Authorize(Policy = "writepolicy")]
    public class RoleManagementController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public RoleManagementController(UserManager<IdentityUser> userManager,
                          RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult Index()
        {
            //Get all users and send to view
            var users = userManager.Users.ToList();
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string userId)
        {
            //Find user by userId
            //Add UserName to ViewBag
            //Get userRole of users and send to view
            var user = await userManager.FindByIdAsync(userId);
            ViewBag.UserName = user.UserName;
            var userRoles = await userManager.GetRolesAsync(user);
            return View(userRoles);
        }

        [HttpGet]
        public IActionResult AddUserToRole()
        {
            var users = userManager.Users.ToList(); //get all users
            var roles = roleManager.Roles.ToList(); //get all roles
            ViewBag.Users = new SelectList(users, "Id", "UserName");
            ViewBag.Roles = new SelectList(roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(string UserId, string RoleName)
        {
            var user = await userManager.FindByIdAsync(UserId);
            await userManager.AddToRoleAsync(user, RoleName); //Assign role to user
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> RemoveUserRole(string role, string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            //Remove role of user using userManager
            var result = await userManager.RemoveFromRoleAsync(user, role);
            return RedirectToAction(nameof(Details), new { userId = user.Id });
        }
    }

}
