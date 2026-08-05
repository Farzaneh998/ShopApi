using MassTransit;
using ShopApi.Contracts.Commands;

namespace ShopApi.Infrastructure.Messaging.Consumers
{
    public class ReserveInventoryConsumer
        : IConsumer<ReserveInventory>
    {
        //اجرای command
        public async Task Consume(
            ConsumeContext<ReserveInventory> context)
        {
            Console.WriteLine("Reserve Inventory");

            Console.WriteLine(context.Message.ProductId);

            Console.WriteLine(context.Message.Quantity);

            await Task.CompletedTask;
        }
    }
}
