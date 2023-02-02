using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrder.Models {

    public class Product {
        public int Id {get; set;}

        public string Name {get; set; } = string.Empty;

        public string Description {get; set;} = string.Empty;

        public decimal Price {get; set;}

        public int CategoryId {get; set;}
        public Category Category {get; set;}

        public string Image {get; set;}

        [Column(TypeName = "decimal(8, 2)")]
        public decimal Rating {get; set;}
    }
}