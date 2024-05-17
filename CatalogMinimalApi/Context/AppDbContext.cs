using CatalogMinimalApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogMinimalApi.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {}

    public DbSet<Product>? Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    // Setting up database tables with FluentAPI
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasKey(c => c.CategoryId);
        modelBuilder.Entity<Category>().Property(c => c.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Category>().Property(c => c.Description).HasMaxLength(150).IsRequired();

        modelBuilder.Entity<Product>().HasKey(c => c.ProductId);
        modelBuilder.Entity<Product>().Property(p => p.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Product>().Property(p => p.Description).HasMaxLength(150);
        modelBuilder.Entity<Product>().Property(p => p.ImageUrl).HasMaxLength(100);
        modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(14, 2);

        modelBuilder.Entity<Product>()
            .HasOne<Category>(c => c.Category)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.ProductId);
    }
}
