using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Models {

    public class OrderHeader {

        public int Id {get; set;}

        public string ApplicationUserId {get; set;}
        public ApplicationUser ApplicationUser {get; set;}
        
        [DataType(DataType.Date)]
        public DateTime OrderDate {get; set;}

        public DateTime PickUpDate {get; set;}
        public double OrderTotal {get; set;}
        public string OrderStatus {get; set;}
        public string PaymentStatus {get; set;}
        public PackingOptions PackOptions {get; set;}
        public decimal TravelDistance {get; set;}
    }
}