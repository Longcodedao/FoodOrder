using FoodOrder.Models;
using FoodOrder.Repository;
using FoodOrder.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrder.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _webHostEnvironment;
        public ProfileController(UserManager<ApplicationUser> userManager, 
                                    ApplicationDbContext context, 
                                    IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var userId = user.Id;
            var CartsOrder = _context.Carts.Include(p => p.OrderHeader)
                                    .Where(p => p.ApplicationUserId == userId)
                                    .Where(p => p.OrderHeader.OrderStatus == "ONGOING")
                                    .ToList();


            if (CartsOrder.Count > 0)
            {
                ViewData["ShowTempOrder"] = true;
            }
            else
            {
                ViewData["ShowTempOrder"] = false;
            }


            ProfileViewModel profile = new ProfileViewModel()
            {
                ImageUrl = user.UserProfileImage,
                Name = user.Name ?? "Unknown",
                PostalCode = user.PostalCode ?? "Unknown",
                PhoneNumber = user.PhoneNumber ?? "XXX-XXXX",
            };
            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitImage(ProfileViewModel profileVM)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (profileVM.Image != null && profileVM.Image.Length > 0)
            {
                var uploadDir = @"Images/Account";
                var fileName = Guid.NewGuid().ToString() + "-" + profileVM.Image.FileName;
                var path = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Account",
                                    fileName);

                await profileVM.Image.CopyToAsync(new FileStream(path, FileMode.Create));

                user.UserProfileImage = "/" + uploadDir + "/" + fileName;

                var result = await _userManager.UpdateAsync(user);

                return RedirectToAction("Index", "Profile");

            }
            else
            {
                return View();
            }
        }


        [HttpPost]
        public async Task<IActionResult> SubmitInformation(ProfileViewModel profileVM)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            user.Name = profileVM.Name;
            user.Address = profileVM.Address;
            user.PhoneNumber = profileVM.PhoneNumber;
            user.PostalCode = profileVM.PostalCode;

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index", "Home");
        }

    }
}
