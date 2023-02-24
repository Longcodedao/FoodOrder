using FoodOrder.Models;
using FoodOrder.Repository;
using FoodOrder.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrder.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private List<Product> _products;
        public HomeController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> OrderConfirmStatus()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userId = user.Id;

            var Order = _context.OrderHeaders.Where(p => p.ApplicationUserId == userId)
                                    .Where(p => p.ConfirmByAdmin == "CONFIRM")
                                    .Where(p => p.FinishPopup == false).FirstOrDefault();

            if (Order != null)
            {
                return true;
            }
            return false;
        }

        public async Task<IActionResult> Index()
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userId = user.Id;
            var CartsOrder = _context.Carts.Include(p => p.OrderHeader)
                                .Where(p => p.ApplicationUserId == userId)
                                .Where(p => p.OrderHeader.OrderStatus == "ONGOING").ToList();

            if (TempData.ContainsKey("orderPlaced"))
            {
                ViewData["ShowOrderNotification"] = true;
                TempData.Remove("orderPlaced");
            }else
            {
                ViewData["ShowOrderNotification"] = false;
            }

            if (CartsOrder.Count > 0)
            {
                ViewData["ShowTempOrder"] = true;
            }else
            {
                ViewData["ShowTempOrder"] = false;
            }

            var popupConfirm = await OrderConfirmStatus();
            if (popupConfirm == true)
            {
                ViewData["ShowConfirmOrder"] = true;
            }else
            {
                ViewData["ShowConfirmOrder"] = false;
            }

            HomePageViewModel homepage = new HomePageViewModel();
            homepage.Products = await _context.Products.Include(p => p.Category).ToListAsync();
            homepage.Categories = await _context.Categories.ToListAsync();

            var ImageUrl = "";
          
            if (user.UserProfileImage == null)
                ImageUrl = "/Images/Account/profile-gray.jpg";
            else
                ImageUrl = user.UserProfileImage;

            homepage.ImageUrl = ImageUrl;
            return View(homepage);
        }

        [HttpGet]
        public async Task<IActionResult> SearchBar()
        {
            

            string stringResult = HttpContext.Request.Query["stringResult"].ToString();
            if (!string.IsNullOrEmpty(stringResult))
            {

                var user = await _userManager.GetUserAsync(HttpContext.User);
                var userId = user.Id;
                var CartsOrder = _context.Carts.Include(p => p.OrderHeader)
                                    .Where(p => p.ApplicationUserId == userId)
                                    .Where(p => p.OrderHeader.OrderStatus == "ONGOING").ToList();
                var products = await _context.Products.Where(p => p.Name.ToLower().Contains(stringResult.ToLower())).ToListAsync();

                if (CartsOrder.Count > 0)
                {
                    ViewData["ShowTempOrder"] = true;
                }
                else
                {
                    ViewData["ShowTempOrder"] = false;
                }


                if (user.UserProfileImage == null)
                    ViewBag.ImageUrl = "/Images/Account/profile-gray.jpg";
                else
                    ViewBag.ImageUrl = user.UserProfileImage;

                ViewBag.Purpose = "Search";
                ViewBag.Category = stringResult;
                return View("CategoryList", products);
            }

            return RedirectToAction("Index", "Home");


        }


        public async Task<IActionResult> CategoryList(string Categoryinp)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userId = user.Id;
            var CartsOrder = _context.Carts.Include(p => p.OrderHeader)
                                .Where(p => p.ApplicationUserId == userId)
                                .Where(p => p.OrderHeader.OrderStatus == "ONGOING").ToList();

            if (CartsOrder.Count > 0)
            {
                ViewData["ShowTempOrder"] = true;
            }
            else
            {
                ViewData["ShowTempOrder"] = false;
            }

            _products = await _context.Products.Include(p => p.Category)
                                .Where(c => c.Category.Title == Categoryinp).ToListAsync();
            var categoryList = _products;


            if (user.UserProfileImage == null)
                ViewBag.ImageUrl = "/Images/Account/profile-gray.jpg";
            else
                ViewBag.ImageUrl = user.UserProfileImage;
            ViewBag.Purpose = "Category";
            ViewBag.Category = Categoryinp;
            return View(categoryList);

        }

        [HttpPost]
        public async Task<IActionResult> CategoryList(string Categoryinp, int sortPrice)
        {

            _products = await _context.Products.Include(p => p.Category)
                                .Where(c => c.Category.Title == Categoryinp).ToListAsync();
            var sortedArr = new List<Product>();

            // Sort Price

            switch (sortPrice)
            {
                case 1:
                    sortedArr = _products.OrderBy(p => p.Price).ToList();
                    break;
                case 2:
                    sortedArr = _products.OrderByDescending(p => p.Price).ToList();
                    break;

                case 3:
                    sortedArr = _products.OrderBy(p => p.Rating).ToList();
                    break;

                case 4:
                    sortedArr = _products.OrderByDescending(p => p.Rating).ToList();
                    break;
            }
            


            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (user.UserProfileImage == null)
                ViewBag.ImageUrl = "/Images/Account/profile-gray.jpg";
            else
                ViewBag.ImageUrl = user.UserProfileImage;

            ViewBag.Category = Categoryinp;


            
            var userId = user.Id;
            var CartsOrder = _context.Carts.Include(p => p.OrderHeader)
                                .Where(p => p.ApplicationUserId == userId)
                                .Where(p => p.OrderHeader.OrderStatus == "ONGOING").ToList();

            if (CartsOrder.Count > 0)
            {
                ViewData["ShowTempOrder"] = true;
            }
            else
            {
                ViewData["ShowTempOrder"] = false;
            }
            return View(sortedArr);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
