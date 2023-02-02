using System.ComponentModel.DataAnnotations;


namespace FoodOrder.Models {

    public class Category {

        [Key]
        public int Id {get; set;}

        [Required(ErrorMessage = "Please enter a category")]
        public string Title {get; set;} = string.Empty;
   
        public IEnumerable<Product> Products {get; set;}
    }
}