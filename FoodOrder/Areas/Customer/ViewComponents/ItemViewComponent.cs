using FoodOrder.Models;
using FoodOrder.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrder.Areas.Customer.ViewComponents
{
    [Area("Customer")]
    public class ItemViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;


        public ItemViewComponent(UserManager<ApplicationUser> userManager,
                            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userId = user.Id;

            var CartsOrder = _context.Carts.Include(p => p.OrderHeader)
                                        .Where(p => p.ApplicationUserId == userId)
                                        .Where(p => p.OrderHeader.OrderStatus == "ONGOING")
                                            .ToList();
            var totalOrders = 0;

            foreach (var cart in CartsOrder)
            {
                totalOrders += cart.Count;
            }

            return View(totalOrders);

        }
    }
}
