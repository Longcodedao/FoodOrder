using FoodOrder.Models;
using FoodOrder.Repository;
using FoodOrder.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace FoodOrder.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;


        public CartController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        
        public CartViewModel InitializeOrder(List<Cart> ListOrderCart, OrderHeader orderHeader)
        {
            CartViewModel cartVM = new CartViewModel()
            {
                ListOfCart = ListOrderCart,
                OrderHeader = orderHeader,
            };

            return cartVM;
        }

        public async Task<IActionResult> Index()
        {

            var user = await _userManager.GetUserAsync(User);
            var userID = user.Id;

            var orderHeaderList =  _context.OrderHeaders
                                    .Where(p => p.ApplicationUserId == userID)
                                    .Where(p => p.OrderStatus == "ONGOING").ToList();
            
            if (orderHeaderList.Count > 0)
            {
                var orderHeader = orderHeaderList[0];
                var ListOfCarts = await _context.Carts.Include(p => p.Product)
                                                        .Where(p => p.OrderHeaderId == orderHeader.Id)
                                                        .ToListAsync();
                orderHeader.NumItems = 0;
                orderHeader.OrderTotal = 0;

                if (ListOfCarts.Count > 0)
                {
                    ViewData["ShowTempOrder"] = true;
                }else
                {
                    ViewData["ShowTempOrder"] = false;
                }

                foreach (var cart in ListOfCarts)
                {
                    orderHeader.NumItems += cart.Count;
                    orderHeader.OrderTotal += cart.Count * cart.Product.Price;
                }

                _context.OrderHeaders.Update(orderHeader);
                await _context.SaveChangesAsync();

                var cartViewModel = InitializeOrder(ListOfCarts, orderHeader);
                ViewBag.ImageUrl = user.UserProfileImage;
                return View(cartViewModel);
            }
            
            return RedirectToAction("NotFoundShopping");
        }

        public IActionResult NotFoundShopping()
        {
            return View();
        }
        public async Task<IActionResult> DecreaseProduct(int Cartid)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == Cartid);
            cart.Count -= 1;

            if (cart.Count == 0)
            {
                _context.Carts.Remove(cart);
            }
            else
            {
                _context.Carts.Update(cart);
            }
            
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddProduct(int Cartid)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == Cartid);
            cart.Count += 1;
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        [HttpPost]

        public async Task<IActionResult> SubmitAddress(CartViewModel cartVM)
        {

            var user = await _userManager.GetUserAsync(User);
            var userID = user.Id;

            var orderHeaderList = _context.OrderHeaders
                                    .Where(p => p.ApplicationUserId == userID)
                                    .Where(p => p.OrderStatus == "ONGOING").ToList();

            var orderHeader = orderHeaderList[0];

            orderHeader.AddressReceive = cartVM.OrderHeader.AddressReceive;

            _context.OrderHeaders.Update(orderHeader);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> SubmitOptions(CartViewModel cartVM)
        {
            var user = await _userManager.GetUserAsync(User);
            var userID = user.Id;

            var orderHeaderList = _context.OrderHeaders
                                    .Where(p => p.ApplicationUserId == userID)
                                    .Where(p => p.OrderStatus == "ONGOING").ToList();

            var orderHeader = orderHeaderList[0];

            orderHeader.ShopOptions = cartVM.OrderHeader.ShopOptions;

            switch (orderHeader.ShopOptions)
            {
                case FoodOrder.Models.shoppingOptions.Basic:
                    orderHeader.PackingMoney = 8000;
                    break;
                case FoodOrder.Models.shoppingOptions.Standard:
                    orderHeader.PackingMoney = 12000;
                    break;
                case FoodOrder.Models.shoppingOptions.Premium:
                    orderHeader.PackingMoney = 18000;
                    break;
            }
                
            
            _context.OrderHeaders.Update(orderHeader);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Finish()
        {
            var user = await _userManager.GetUserAsync(User);
            var userID = user.Id;

            var orderHeaderList = _context.OrderHeaders
                                    .Where(p => p.ApplicationUserId == userID)
                                    .Where(p => p.OrderStatus == "ONGOING").ToList();

            var orderHeader = orderHeaderList[0];

            orderHeader.BillTotal = (int) orderHeader.OrderTotal + orderHeader.PackingMoney;
            orderHeader.OrderStatus = "FINISHED";

            _context.OrderHeaders.Update(orderHeader);

            await _context.SaveChangesAsync();

            return View();
        }

    }
}
