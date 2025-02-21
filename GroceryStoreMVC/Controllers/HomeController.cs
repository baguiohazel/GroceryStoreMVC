using System.Diagnostics;
using GroceryStoreMVC.Data; // Add this for ApplicationDbContext
using GroceryStoreMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GroceryStoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context; // Add ApplicationDbContext

        public HomeController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context) // Inject ApplicationDbContext
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context; // Initialize ApplicationDbContext
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity!.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user!);
                var userName = user!.UserName; // You can use user.Email if needed

                ViewData["UserName"] = userName;

                if (roles.Contains("Admin"))
                {
                    ViewData["Role"] = "Admin";
                    ViewData["WelcomeMessage"] = "Welcome Admin, you have full control over the system.";
                }
                else if (roles.Contains("InventoryManager"))
                {
                    ViewData["Role"] = "Inventory Manager";
                    ViewData["WelcomeMessage"] = "Welcome Inventory Manager, keep track of stocks efficiently.";
                }
                else if (roles.Contains("Cashier"))
                {
                    ViewData["Role"] = "Cashier";
                    ViewData["WelcomeMessage"] = "Welcome Cashier, process customer transactions smoothly.";
                }
            }
            else
            {
                ViewData["Role"] = "Guest";
                ViewData["WelcomeMessage"] = "Welcome to our Grocery Store! Please log in to continue.";
            }

            return View();
        }

        public IActionResult Dashboard(DbSet<Order> order)
        {
            if (_context.Orders == null)
            {
               
                _context.Orders = order;
            }

            // Create a new Dashboard model and populate it with data
            var dashboard = new Dashboard
            {
                TotalProducts = _context.Products.Count(),
                TotalCustomers = _context.Customers.Count(),
                TotalSuppliers = _context.Suppliers.Count(),
                LowStockItems = _context.Products.Count(p => p.StockThreshold < 10), // Example threshold
                TotalSalesToday = (int)_context.Orders
                    .OfType<Order>()
                    .Where(o => o.OrderDate.Date == DateTime.Today)
                    .Sum(o => o.TotalAmount) // Assuming TotalAmount is the sales amount
            };

            // Pass the model to the view
            return View(dashboard);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}