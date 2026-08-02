using MassTransit;
using ShopApi.Contracts.Events;

namespace ShopApi.Infrastructure.Messaging.Consumers
{

    public class ProductCreatedConsumer : IConsumer<ProductCreated>
    {
        public async Task Consume(ConsumeContext<ProductCreated> context)
        {
            Console.WriteLine("========== EVENT RECEIVED ==========");
            Console.WriteLine($"Id : {context.Message.ProductId}");
            Console.WriteLine($"Name : {context.Message.Name}");
            Console.WriteLine($"Price : {context.Message.Price}");
            await Task.CompletedTask;
            //Console.WriteLine("Consumer Running");
            //throw new Exception("Test Retry");

        }
    }
}
