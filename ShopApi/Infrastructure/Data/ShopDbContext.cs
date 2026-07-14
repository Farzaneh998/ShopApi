using Microsoft.EntityFrameworkCore;
using ShopApi.Domain.Entities;

namespace ShopApi.Infrastructure.Data;

public class ShopDbContext : DbContext
{
    public ShopDbContext(DbContextOptions<ShopDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        //Fluent API to database level
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Price)
                .HasPrecision(18, 2);
        });
    }
}