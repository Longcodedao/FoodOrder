using FoodOrder.Models;

namespace FoodOrder.ViewModels
{
    public class OrderViewModelAdmin
    {
        public OrderHeader OrderHeader { get; set; }
        public List<Cart> Carts { get; set; }
    }
}
