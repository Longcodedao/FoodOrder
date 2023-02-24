using FoodOrder.Models;
using FoodOrder.Repository;
using FoodOrder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodOrder.Areas.Admin.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _webHostEnvironment;
        public ProductController(ApplicationDbContext context,
                IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var products = _context.Products.Include(x => x.Category)
                .Select(model => new ProductViewModel()
                {
                    Id = model.Id,
                    Title = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    Rating = model.Rating,
                }).ToList();
            
            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ProductViewModel vm = new ProductViewModel();
            ViewBag.Category = new SelectList(_context.Categories, "Id",
                                "Title");
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductViewModel vm)
            
        {
            Product model = new Product();
            if (ModelState.IsValid)
            {
                if(vm.ImageUrl != null && vm.ImageUrl.Length > 0)
                {
                    var uploadDir = @"Images/Product";
                    var fileName = Guid.NewGuid().ToString() + "-" + vm.ImageUrl.FileName;
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Product",
                                        fileName);

                    await vm.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    model.Image = "/" + uploadDir + "/" + fileName;
                }
                model.Price = vm.Price;
                model.Description = vm.Description;
                model.Name = vm.Title;
                model.CategoryId = vm.CategoryId;
                model.DeliverMinutes = vm.DeliverMinutes;
            
                _context.Products.Add(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            ViewBag.Category = new SelectList(_context.Categories, "Id",
                                "Title");
            ProductViewModel vm = new ProductViewModel()
            {
                Title = product.Name,
                Description = product.Description,
                Price = product.Price,
                DeliverMinutes = product.DeliverMinutes,
            };
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(ProductViewModel vm)
        {
            
            if (ModelState.IsValid)
            {
                var model = await _context.Products.FirstOrDefaultAsync(_ => _.Id == vm.Id);
                /*var oldPath = Path.Combine(_webHostEnvironment.WebRootPath, model.Image);

                var totalPath = Path.Combine(Directory.GetCurrentDirectory(), "MVC_App", "FoodOrder", "FoodOrder", "wwwroot", model.Image);
                System.IO.File.Delete(totalPath);

                if (vm.ImageUrl != null && vm.ImageUrl.Length > 0)*/
                {
                    var uploadDir = @"Images/Product";
                    var fileName = Guid.NewGuid().ToString() + "-" + vm.ImageUrl.FileName;
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Product",
                                        fileName);

                    await vm.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    model.Image = "/" + uploadDir + "/" + fileName;
                }
                model.Price = vm.Price;
                model.Description = vm.Description;
                model.Name = vm.Title;
                model.CategoryId = vm.CategoryId;
                model.Category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == vm.CategoryId);
                model.DeliverMinutes = vm.DeliverMinutes;

                _context.Products.Update(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products.Include(p => p.Category)
                        .FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                ViewBag.Image = product.Image;
                ProductViewModel prod = new ProductViewModel()
                {
                    Id = product.Id,
                    Title = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    Category = product.Category,
                    DeliverMinutes = product.DeliverMinutes,
                    Rating = product.Rating
                };
                return View(prod);
            }
            return View();
            
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.Include(p => p.Category)
                        .FirstOrDefaultAsync(p => p.Id == id);

            if (product != null)
            {
                ViewBag.Image = product.Image;
                ProductViewModel prod = new ProductViewModel()
                {
                    Id = product.Id,
                    Title = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId,
                    Category = product.Category,
                    Rating = product.Rating
                };
                return View(prod);
            }
            return View();

        }


        [HttpPost]
        public async Task<IActionResult> Delete(ProductViewModel vm)
        {
            var product = new Product()
            {
                Id = vm.Id,
                Name = vm.Title,
                Description = vm.Description,
                Price = vm.Price,
                CategoryId = vm.CategoryId,
                Category = vm.Category
            };

            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
