using System.ComponentModel.DataAnnotations;

namespace FoodOrder.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public IFormFile ImageUrl { get; set; }
    }
}
