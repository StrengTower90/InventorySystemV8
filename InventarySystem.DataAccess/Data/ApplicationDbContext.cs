using InventarySystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace InventarySystem.DataAccess.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Store> Stores { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Brand> Brand { get; set; }
    public DbSet<Product> Product { get; set; }
    public DbSet<UserApplication> UserApplication { get; set; }

    public DbSet<StoreProduct> StoresProducts { get; set; }

    public DbSet<Inventory> Inventories { get; set; }

    public DbSet<InventoryDetails> InventoryDetails { get; set; }
    public DbSet<KardexInventory> KardexInventory { get; set; }
    public DbSet<Company> Company { get; set; }
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
