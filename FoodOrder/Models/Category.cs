using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        [Required]
        public string Title { get; set; }   

        public IEnumerable<Product> Products { get; set; }
    }
}