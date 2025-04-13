using HospitalManagement.Models;
using HospitalManagement.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers
{
    [Authorize (Roles = "Admin")]
    public class RoleController : Controller
    {
        public readonly RoleManager<IdentityRole> roleManger;
        public RoleController(RoleManager<IdentityRole> roleManger)
        {
            this.roleManger = roleManger;
        }
        public IActionResult AddRole() 
        {
            return View("AddRole");
        }
        [HttpPost]
        public async Task<IActionResult> SaveRole(RoleViewModel roleVM)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole();
                role.Name = roleVM.RoleName;
                IdentityResult result = await roleManger.CreateAsync(role);
                if (result.Succeeded)
                {
                    ViewBag.sucess = true;
                    return View("AddRole");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("AddRole", roleVM);
        }
    }
}
