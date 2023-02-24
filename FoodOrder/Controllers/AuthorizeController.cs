using Microsoft.AspNetCore.Mvc;
using FoodOrder.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using FoodOrder.Models;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace FoodOrder.Controllers
{
    public class AuthorizeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        
        private readonly SignInManager<ApplicationUser> signInManager;
        
        private readonly RoleManager<IdentityRole> roleManager;

        public AuthorizeController(UserManager<ApplicationUser> userManager,
                    
            SignInManager<ApplicationUser> signInManager,
            
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        [AllowAnonymous]

        public IActionResult Register1()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]

        public async Task <IActionResult> Register1(Register reg)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    UserName = reg.EmailAddress,
                    Email = reg.EmailAddress,
                    UserProfileImage = "/Images/Account/profile-gray.jpg"
                };
                var password = reg.Password;

                var result = await userManager.CreateAsync(user, password);

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    var adminRole = new IdentityRole("Admin");
                    await roleManager.CreateAsync(adminRole);
                }


                if(result.Succeeded)
                {
                    
                    if (user.UserName == "admin@google.com")
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                        await signInManager.SignInAsync(user, false);
                     
                        
                    }

                    await signInManager.SignInAsync(user, false);

                    return RedirectToAction("UserInfo", "Authorize");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            return View();
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login1()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login1(Login log)
        {
            if(ModelState.IsValid)
            {
                var identityResult = await signInManager.PasswordSignInAsync(log.Email, log.Password, log.RememberMe, false);
                
                if (identityResult.Succeeded)
                {
                    var user = await userManager.FindByEmailAsync(log.Email);
                    if (await userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    return RedirectToAction("Index", "Home", new {area = "Customer"});
                }

                ModelState.AddModelError("", "Username or Password Error");
            }
            return View();
        }


        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<IActionResult> UserInfo()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            ProfileViewModel profileVM = new ProfileViewModel();
            return View(profileVM);
           
        }

        [HttpPost]
        public async Task<IActionResult> UserInfo(ProfileViewModel profileVM)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);

            user.Name = profileVM.Name;
            user.PhoneNumber= profileVM.PhoneNumber;
            user.PostalCode = profileVM.PostalCode;
            user.Address = profileVM.Address;

            await userManager.UpdateAsync(user);

            var IsInRole = await userManager.IsInRoleAsync(user, "Admin");
            if (IsInRole)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }
    }
}
