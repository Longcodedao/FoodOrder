using System.ComponentModel.DataAnnotations;

namespace FoodOrder.Models {

    public class CartLine {

        [Key]
        public int Id {get; set;}

        public int ProductId {get; set;}

        public Product Product {get; set;} = new();
        
        [Required, MinLength(1)]
        public int Count {get; set;}

    }

    public class Cart {

        public string ApplicationUserId {get; set;}
        public ApplicationUser ApplicationUser {get; set;}
        public List <CartLine> Lines {get; set;} = new List<CartLine>();

        public virtual void AddItem(Product product, int count) {
            CartLine line = Lines
                .Where(p => p.Product.Id == product.Id)
                .FirstOrDefault();

            if (line == null){
                Lines.Add(new CartLine {
                    ProductId = product.Id,
                    Product = product,
                    Count = count
                });
            } else {
                line.Count += count;
            }
        }

        public virtual void RemoveLine(Product product) => 
            Lines.RemoveAll(l => l.Product.Id == product.Id);
    
        public decimal ComputeTotalValue() => 
            Lines.Sum(e => e.Product.Price * e.Count);
    
        public virtual void Clear() => Lines.Clear();
    }

    
}