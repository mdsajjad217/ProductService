using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProductService.Infrastructure.Persistence;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options)
        : base(options) { }
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Price).IsRequired();
        });

        modelBuilder.Entity<Product>().HasData(
        new Product("Laptop", 1200m),
        new Product("Keyboard", 80m));
    }
}
