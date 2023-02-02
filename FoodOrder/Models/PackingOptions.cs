using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Models {

    public class PackingOptions {

        [Key]
        public int Id {get; set;}
        public string Name {get; set;}
        public double Price {get; set;}
        public string Image {get; set;}
    }
}