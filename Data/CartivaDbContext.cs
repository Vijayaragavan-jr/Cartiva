using Cartiva.Models;
using Microsoft.EntityFrameworkCore;

namespace Cartiva.Data;

public partial class CartivaDbContext : DbContext
{
    public CartivaDbContext(DbContextOptions<CartivaDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .Property(x => x.Role)
            .HasMaxLength(20)
            .HasDefaultValue("Customer");

        modelBuilder.Entity<Product>()
            .Property(x => x.Price)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Cart>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId);

        modelBuilder.Entity<CartItem>()
            .HasOne<Cart>()
            .WithMany()
            .HasForeignKey(x => x.CartId);

        modelBuilder.Entity<CartItem>()
            .HasOne<Product>()
            .WithMany()
            .HasForeignKey(x => x.ProductId);

        modelBuilder.Entity<CartItem>()
            .HasIndex(x => new { x.CartId, x.ProductId })
            .IsUnique();
    }
}