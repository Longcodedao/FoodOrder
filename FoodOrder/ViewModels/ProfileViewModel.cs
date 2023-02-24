namespace FoodOrder.ViewModels
{
    public class ProfileViewModel
    {
        public string ImageUrl { get; set; }
        public IFormFile Image { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
       
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
