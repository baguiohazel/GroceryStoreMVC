namespace GroceryStoreMVC.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int StockThreshold { get; set; } 
        public int? CategoryId { get; set; }
        public int? SupplierId { get; set; }
        public Category? Categories { get; set; }
        public Supplier? Suppliers { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }


    }
}
