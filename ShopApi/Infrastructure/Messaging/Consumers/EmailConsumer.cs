using MassTransit;
using ShopApi.Contracts.Events;

namespace ShopApi.Infrastructure.Messaging.Consumers
{
    public class EmailConsumer:IConsumer<ProductCreated>
    {
        public async Task Consume(ConsumeContext<ProductCreated> consumeContext)
        {
            Console.WriteLine("sendEmail");
            await Task.CompletedTask;
        }
    }
}
