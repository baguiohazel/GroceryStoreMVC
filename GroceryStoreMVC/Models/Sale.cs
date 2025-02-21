namespace GroceryStoreMVC.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public DateTime SaleDate { get; set; }
        public int CustomerId { get; set; }
        public double TotalAmount { get; set; }
        public Customer? Customers { get; set; }
        public ICollection<Product>? Products { get; set; }
    }
}
