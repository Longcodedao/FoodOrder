using FoodOrder.Repository;
using FoodOrder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace FoodOrder.Areas.Admin.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _webHostEnvironment;

        public OrderController(ApplicationDbContext context,
                IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var orders = _context.OrderHeaders
                                .Where(p => p.ConfirmByAdmin == "NOT CONFIRM")
                                .Where(p => p.OrderStatus == "FINISHED");
                                         
            return View(orders);
        }

        
        public async Task<IActionResult> Details (int id)
        {
            var order = await _context.OrderHeaders.FirstOrDefaultAsync(p => p.Id == id);
            var orderId = order.Id;

            var ListOfCarts = _context.Carts
                                    .Include(p => p.Product)
                                    .Where(p => p.OrderHeaderId == orderId)
                                    .ToList();

            OrderViewModelAdmin orderVM = new OrderViewModelAdmin()
            {
                OrderHeader = order,
                Carts = ListOfCarts,
            };

            return View(orderVM);
        }


        public async Task<IActionResult> ConfirmOrder (int id)
        {
            var order = await _context.OrderHeaders.FirstOrDefaultAsync(p => p.Id == id);
            var orderId = order.Id;

            order.ConfirmByAdmin = "CONFIRM";

            _context.OrderHeaders.Update(order);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Index");
        }
    }
}
