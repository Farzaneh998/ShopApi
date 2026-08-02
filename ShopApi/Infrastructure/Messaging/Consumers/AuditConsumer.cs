using MassTransit;
using ShopApi.Contracts.Events;

namespace ShopApi.Infrastructure.Messaging.Consumers
{
    public class AuditConsumer : IConsumer<ProductCreated>
    {
        public async Task Consume(
            ConsumeContext<ProductCreated> context)
        {
            Console.WriteLine("Audit Saved");

            await Task.CompletedTask;
        }
    }
}
