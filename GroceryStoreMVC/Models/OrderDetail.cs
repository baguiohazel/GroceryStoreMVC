namespace GroceryStoreMVC.Models
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int OrderId { get; set; } 
        public int ProductId { get; set; } 
        
        public Order? Orders { get; set; }
        public Product? Products { get; set; } 

        public int Quantity { get; set; }
        public double Subtotal { get; set; }
    }
}
