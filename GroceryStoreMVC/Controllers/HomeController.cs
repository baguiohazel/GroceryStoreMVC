using System.Diagnostics;
using GroceryStoreMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GroceryStoreMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
    }
}