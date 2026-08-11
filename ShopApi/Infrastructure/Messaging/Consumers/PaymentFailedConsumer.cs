using MassTransit;
using Microsoft.EntityFrameworkCore;
using ShopApi.Contracts.Events;
using ShopApi.Infrastructure.Data;

namespace ShopApi.Infrastructure.Messaging.Consumers
{
    public class PaymentFailedConsumer
        : IConsumer<PaymentFailed>
    {
        private readonly ShopDbContext _context;

        public PaymentFailedConsumer(
            ShopDbContext context)
        {
            _context = context;
        }

        public async Task Consume(
            ConsumeContext<PaymentFailed> context)
        {
            var message = context.Message;

            var saga = await _context.OrderSagaStates
                .FirstOrDefaultAsync(
                    x => x.CorrelationId == message.CorrelationId);

            if (saga is null)
                return;

            saga.CurrentState = "PaymentFailed";

            var order = await _context.Orders
                .FirstOrDefaultAsync(
                    x => x.Id == message.OrderId);

            if (order is not null)
            {
                order.Status = "Cancelled";
            }

            await _context.SaveChangesAsync();
        }
    }
}
