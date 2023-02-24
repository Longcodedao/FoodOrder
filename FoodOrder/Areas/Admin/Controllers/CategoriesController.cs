using FoodOrder.Models;
using FoodOrder.Repository;
using FoodOrder.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodOrder.Areas.Admin.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    [Area("Admin")]
    public class CategoriesController : Controller
    {

        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _webHostEnvironment;

        public CategoriesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        private IEnumerable<Category> Categories => _context.Categories;

       
        public IActionResult Index()
        {
            var listFromDb = Categories.ToList().Select(
                x => new CategoryViewModel()
                {
                    Id = x.Id,
                    Title = x.Title
                }).ToList();
            return View(listFromDb);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var catFromDb = await _context.Categories.FindAsync(id);

            ViewBag.ImageUrl = catFromDb.ImageUrl;
            var catModel = new CategoryViewModel()
            {
                Id = catFromDb.Id,
                Title = catFromDb.Title
            };

            return View(catModel);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View("Create", new CategoryViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel vm)
        {
            Category model = new Category();
            if (ModelState.IsValid)
            {

                if (vm.ImageUrl != null && vm.ImageUrl.Length > 0)
                {
                    var uploadDir = @"Images/Category";
                    var fileName = Guid.NewGuid().ToString() + "-" + vm.ImageUrl.FileName;
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Category",
                                        fileName);

                    await vm.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    model.ImageUrl = "/" + uploadDir + "/" + fileName;
                }

                
                model.Id = default;
                model.Title = vm.Title;
                _context.Categories.Add(model);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(p => p.Id == id);

            CategoryViewModel catModel = new CategoryViewModel()
            {
                Id = category.Id,
                Title = category.Title,
            };

            return View(catModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var category = await _context.Categories
                    .FirstOrDefaultAsync(p => p.Id == vm.Id);

                if (vm.ImageUrl != null && vm.ImageUrl.Length > 0)
                {
                    var uploadDir = @"Images/Category";
                    var fileName = Guid.NewGuid().ToString() + "-" + vm.ImageUrl.FileName;
                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Category",
                                        fileName);

                    await vm.ImageUrl.CopyToAsync(new FileStream(path, FileMode.Create));
                    category.ImageUrl = "/" + uploadDir + "/" + fileName;
                }

                if (category != null)
                {
                    category.Title = vm.Title;

                    _context.Categories.Update(category);
                    await _context.SaveChangesAsync();

                }

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Wrong Edit");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Delete (int id)
        {
            var catFromDb = await _context.Categories
                    .FirstOrDefaultAsync(p => p.Id == id);
            ViewBag.ImageUrl = catFromDb.ImageUrl;
            var catModel = new CategoryViewModel()
            {
                Id = catFromDb.Id,
                Title = catFromDb.Title
            };

            return View(catModel);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(CategoryViewModel vm)
        {
            var category = new Category()
            {
                Id = vm.Id,
                Title = vm.Title
            };

            if (category != null)
            {
                _context.Categories.Remove(category);
                
                await _context.SaveChangesAsync();

            }

            return RedirectToAction("Index");
        }
    }
}
