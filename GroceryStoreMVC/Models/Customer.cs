namespace GroceryStoreMVC.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public string? Contact { get; set; } = string.Empty;
        public int LoyaltyPoints { get; set; }
        public ICollection<Order>? Orders { get; set; }
    }
}
