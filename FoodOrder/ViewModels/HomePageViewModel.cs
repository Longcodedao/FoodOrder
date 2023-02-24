using FoodOrder.Models;

namespace FoodOrder.ViewModels
{
    public class HomePageViewModel
    {
        public List<Product> Products { get; set; }

        public List<Category> Categories { get; set; }
        public string ImageUrl { get; set; }

        public int item { get; set; } = 2;
    }
}
