using HospitalManagement.Models;
using HospitalManagement.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManger;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManger,
                SignInManager<ApplicationUser> signInManager) 
        {
            this.userManger = userManger;
            this.signInManager = signInManager;
        }
        public IActionResult SelectRegisterType()
        {
            return View("SelectRegisterType");
        }
        [HttpGet]
        public IActionResult Register() 
        {
           
            //if (string.IsNullOrEmpty(role) || !(role == "Doctor" || role == "Patient" || role == "Staff")) 
            //{
            //    return RedirectToAction("SelectRegisterType");
            //}
            //RegisterUserViewModel model = new RegisterUserViewModel
            //{
            //    RoleName = role // ViewModel
            //};

            return View("Register");
        }
        [HttpPost]
        public async Task<IActionResult> SaveRegister(RegisterUserViewModel UserViewModel)
        {
            if (ModelState.IsValid)
            {
                //Mapping
                var User = new ApplicationUser();
                User.UserName = UserViewModel.UserName;
                User.Email = UserViewModel.UserEmail;
                User.PasswordHash = UserViewModel.Password;

                //Save database
                IdentityResult result = await userManger.CreateAsync(User, UserViewModel.Password);
                if (result.Succeeded)
                {
                    //assign to role
                    await userManger.AddToRoleAsync(User, "Patient");

                    await signInManager.SignInAsync(User, isPersistent: false);

                    return RedirectToAction("Add", "Patient");                    
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View("Register", UserViewModel);
        }
        [HttpGet]
        public IActionResult login()
        {
            return View("login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]//request.form.key[_haidn token]
        public async Task<IActionResult> SaveLogin(LoginUserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                //check found
                ApplicationUser appUser =
                       await userManger.FindByNameAsync(userViewModel.UserName);
                if (appUser != null)
                {
                    bool found =
                       await userManger.CheckPasswordAsync(appUser, userViewModel.Password);
                    if (found == true)
                    {
                        //create cooki
                        await signInManager.SignInAsync(appUser, userViewModel.RememberMe);
                        return RedirectToAction("Privacy", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "UserName or password Wrong");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "UserName or password Wrong");
                }
            }
            return View("login", userViewModel);
        }
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return View("login");
        }

    }
}
