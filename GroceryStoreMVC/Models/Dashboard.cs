namespace GroceryStoreMVC.Models
{
    internal class Dashboard
    {
        public int TotalProducts { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalSuppliers { get; set; }
        public int LowStockItems { get; set; }
        public int TotalSalesToday { get; set; }
    }
}