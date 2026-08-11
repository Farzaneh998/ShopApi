using MassTransit;
using Microsoft.EntityFrameworkCore;
using ShopApi.Contracts.Events;
using ShopApi.Infrastructure.Data;

namespace ShopApi.Infrastructure.Messaging.Consumers
{
    public class PaymentCompletedConsumer
        : IConsumer<PaymentCompleted>
    {
        private readonly ShopDbContext _context;

        public PaymentCompletedConsumer(
            ShopDbContext context)
        {
            _context = context;
        }

        public async Task Consume(
            ConsumeContext<PaymentCompleted> context)
        {
            var message = context.Message;

            var order = await _context.Orders
                .FirstOrDefaultAsync(
                    x => x.Id == message.OrderId);

            if (order is null)
                return;

            order.Status = "Completed";

            await _context.SaveChangesAsync();

            Console.WriteLine(
                $"Order {order.Id} completed.");
        }
    }
}
