using MassTransit;
using Microsoft.EntityFrameworkCore;
using ShopApi.Domain.Entities;
using ShopApi.Infrastructure.Messaging.Saga;

namespace ShopApi.Infrastructure.Data;

public class ShopDbContext : DbContext
{
    public ShopDbContext(DbContextOptions<ShopDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens=> Set<RefreshToken>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<OrderSagaState> OrderSagaStates => Set<OrderSagaState>();
    public DbSet<Order> Orders => Set<Order>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        #region(map sagaEntity)

        modelBuilder.Entity<OrderSagaState>(entity =>
        {
            entity.HasKey(x => x.CorrelationId);

            entity.Property(x => x.CurrentState)
                .HasMaxLength(100);

            entity.Property(x => x.OrderId)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            entity.Property(x => x.Amount)
                .IsRequired();
        });
        #endregion



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