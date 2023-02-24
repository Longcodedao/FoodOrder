using FoodOrder.Models;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrder.ViewModels
{

    [Area("Customer")]
    public class CartViewModel
    {
        public List<Cart> ListOfCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
