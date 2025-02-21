using GroceryStoreMVC.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

// Register RoleManager and UserManager in DI
builder.Services.AddScoped<RoleManager<IdentityRole>>();
builder.Services.AddScoped<UserManager<IdentityUser>>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Create Roles and Users on Startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateRolesAndUsers(services);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
    }

    private static async Task CreateRolesAndUsers(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string[] roleNames = { "Admin", "InventoryManager", "Cashier" };

    // Ensure each role exists
    foreach (var roleName in roleNames)
    {
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // 🔹 Ensure Admin User Exists
    await CreateUserIfNotExists(userManager, "admin@admin.com", "Admin@123", "Admin");

    // 🔹 Ensure Inventory Manager User Exists
    await CreateUserIfNotExists(userManager, "manager@manager.com", "Manager@123", "InventoryManager");



    var cashierAccounts = new Dictionary<string, string>
        {
                 { "cashier1@cashier.com", "Cashier@123" },
                 { "cashier2@cashier.com", "Cashier@234" },
                 { "cashier3@cashier.com", "Cashier@345" },
                 { "cashier4@cashier.com", "Cashier@456" }
         };

    foreach (var account in cashierAccounts)
    {
        await CreateUserIfNotExists(userManager, account.Key, account.Value, "Cashier");
    }

}

/// <summary>
/// Helper function to check if a user exists and create them if not.
/// </summary>
private static async Task CreateUserIfNotExists(UserManager<IdentityUser> userManager, string email, string password, string role)
{
    var user = await userManager.FindByEmailAsync(email);
    if (user == null)
    {
        var newUser = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(newUser, password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, role);
        }
    }
}
}