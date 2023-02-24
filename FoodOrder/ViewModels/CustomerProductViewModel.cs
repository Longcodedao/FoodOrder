using FoodOrder.Models;

namespace FoodOrder.ViewModels
{
    public class CustomerProductViewModel
    {
        public Product CurrentProduct { get; set; }

        // Having the same Category
        public List<Product> OtherProducts { get; set; }

        public List<Review> ReviewsProduct { get; set; }
    }
}
