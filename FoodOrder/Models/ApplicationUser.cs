using Microsoft.AspNetCore.Identity;

namespace FoodOrder.Models {
    public class ApplicationUser: IdentityUser {

        public string Name {get; set;} = string.Empty;
        public string Phone {get; set;}
        public string City {get; set;} = string.Empty;      
        public string Address {get; set;} = string.Empty;

        public string PostalCode {get; set;} = string.Empty;
    }
}