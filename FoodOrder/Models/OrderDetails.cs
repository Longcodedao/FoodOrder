namespace FoodOrder.Models {

    public class OrderDetails {

        public int Id {get; set;}
        public int OrderHeaderId {get; set;}
        public OrderHeader OrderHeader { get; set;}
        public Cart Lines {get; set;}

    }
}