using FoodOrder.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodOrder.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile ImageUrl { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; }
 
        public int DeliverMinutes { get; set; }

        public double Rating { get; set; }
    }
}
