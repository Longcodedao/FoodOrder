namespace FoodOrder.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        // Reference to User
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        
        // Reference to Product being reviewed
        public int ProductId { get; set; }
        public Product Product { get; set; }
       
    }
}
