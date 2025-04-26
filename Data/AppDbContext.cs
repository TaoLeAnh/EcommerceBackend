using Microsoft.EntityFrameworkCore;
using Models;

public class AppDbContext : DbContext
{   
    public AppDbContext() {}
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<Category> Categories {get; set;}

    public DbSet<Product> Products {get; set;} 

    public DbSet<Cart> Carts {get; set;}

    public DbSet<CartItem> CartItems {get; set;}

    public DbSet<Order> Orders {get; set;}

    public DbSet<OrderDetail> OrderDetails {get; set;}
}