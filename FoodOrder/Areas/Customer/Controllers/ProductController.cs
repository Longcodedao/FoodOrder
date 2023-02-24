using FoodOrder.Models;
using FoodOrder.Repository;
using FoodOrder.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FoodOrder.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProductController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public List<int> GenerateRandom(int count)
        {
            List<int> random = new List<int>();

            Random genRandom = new Random();
            while (random.Count < 5)
            {
                var num = genRandom.Next(count);
                if (!random.Contains(num))
                {
                    random.Add(num);
                }
            }
            return random;
        }

        public async Task<IActionResult> Index(int id)
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

            var currentProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        
            
                var otherProducts = await _context.Products.Include(p => p.Category)
                                   .Where(p => p.CategoryId == currentProduct.CategoryId).ToListAsync();

                otherProducts.RemoveAll(p => p.Id == currentProduct.Id);

                var listProduct = GenerateRandom(otherProducts.Count);

                var suggestProduct = new List<Product>();
                foreach (var num in listProduct)
                {
                    suggestProduct.Add(otherProducts[num]);
                }

                var reviewProduct = await _context.Reviews.Include(p => p.User).
                                            Where(p => p.ProductId == id).ToListAsync();

                var ProductViewModel = new CustomerProductViewModel()
                {
                    CurrentProduct = currentProduct,
                    OtherProducts = suggestProduct,
                    ReviewsProduct = reviewProduct
                };

                
                if (user.UserProfileImage == null)
                    ViewBag.ImageUrl = "/Images/Account/profile-gray.jpg";
                else
                    ViewBag.ImageUrl = user.UserProfileImage;

                ViewBag.ProductId = currentProduct.Id;
                return View(ProductViewModel);
            
        }



        [HttpPost]
        public async Task<IActionResult> Index(int rating, int ProductId, string review)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentDateTime = DateTime.Now;

            var reviewProduct = new Review()
            {
                Rating = rating,
                Text = review,
                UserId = userId,
                ProductId = ProductId,
                CreatedAt= currentDateTime,
            };

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == ProductId);
            product.Rating = Math.Round(((product.Rating + rating) / 2), 1);

            _context.Products.Update(product);
            _context.Reviews.Add(reviewProduct);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", ProductId);
        }



        public async Task<IActionResult> OrderNow(int Productid)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userId = user.Id;
            
            var listCart = _context.Carts.Include(p => p.OrderHeader)
                                .Where(p => p.ApplicationUserId== userId)
                                .Where(p=> p.ProductId == Productid)
                                .Where(p => p.OrderHeader.OrderStatus == "ONGOING").ToList();

            var orderHeaderList = _context.Carts.Include(p => p.OrderHeader).
                                Where(p => p.OrderHeader.ApplicationUserId == userId).
                                Where(p => p.OrderHeader.OrderStatus == "ONGOING").ToList();

            var orderHeader = new OrderHeader();

            if (orderHeaderList.Count == 0)
            {

                orderHeader = new OrderHeader() {
                    ApplicationUserId = userId,
                    Name = user.Name,
                    Phone = user.PhoneNumber,
                    OrderStatus = "ONGOING",
                    OrderDate = DateTime.Now
                };

                _context.OrderHeaders.Add(orderHeader);
                await _context.SaveChangesAsync();

            }else
            {
                orderHeader = orderHeaderList[0].OrderHeader;
            }


            if (listCart.Count == 0)
            {
                Cart newCart = new Cart()
                {
                    ProductId = Productid,
                    ApplicationUserId = userId,
                    Count = 1,
                    OrderHeaderId = orderHeader.Id,
                }; 
                _context.Carts.Add(newCart);
            }else
            {
                var currentCart = listCart[0];
                currentCart.Count += 1;
                _context.Carts.Update(currentCart);
            }

            await _context.SaveChangesAsync();

            TempData["orderPlaced"] = true;
            return RedirectToAction("Index", "Home");
        }

    }
}
