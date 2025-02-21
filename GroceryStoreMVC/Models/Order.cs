using Microsoft.AspNetCore.Identity;

namespace GroceryStoreMVC.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } 
        public int CustomerId { get; set; } 
        public double TotalAmount { get; set; } 
        public Customer? Customers { get; set; } 
        public IdentityUser? Users { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; } 

       
    }
}
