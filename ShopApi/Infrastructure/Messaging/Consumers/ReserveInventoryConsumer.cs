using MassTransit;
using Microsoft.EntityFrameworkCore;
using ShopApi.Contracts.Commands;
using ShopApi.Contracts.Events;
using ShopApi.Infrastructure.Data;
using ShopApi.Infrastructure.Messaging.Saga;

namespace ShopApi.Infrastructure.Messaging.Consumers
{
    public class ReserveInventoryConsumer
        : IConsumer<ReserveInventory>
    {
        private readonly ShopDbContext _Context;
        public ReserveInventoryConsumer(ShopDbContext shopDbContext)
        {
            _Context = shopDbContext;       
        }
        public async Task Consume(
            ConsumeContext<ReserveInventory> context)
        {
            var message = context.Message;

            //  رزرو موجودی
            Console.WriteLine(
                $"Inventory reserved for Order {message.OrderId}");


            //saga state
            var massage = context.Message;
            //var saga = new OrderSagaState
            //{
            //    CorrelationId = massage.CorrelationId,
            //    OrderId = massage.OrderId,
            //    CurrentState = "InventoryResrevd",
            //    CreatedAt = DateTime.UtcNow
            //};
            //_Context.OrderSagaStates.Add(saga);
            //await _Context.SaveChangesAsync();

            await context.Publish(
                new InventoryReserved(
                    message.CorrelationId,
                    message.OrderId));


            //Console.WriteLine("Reserve Inventory");
            //Console.WriteLine(context.Message.ProductId);
            //Console.WriteLine(context.Message.Quantity);
            //await Task.CompletedTask;
        }
    }
}
