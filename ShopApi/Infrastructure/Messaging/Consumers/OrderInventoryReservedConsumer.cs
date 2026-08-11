using MassTransit;
using Microsoft.EntityFrameworkCore;
using ShopApi.Contracts.Commands;
using ShopApi.Contracts.Events;
using ShopApi.Infrastructure.Data;
using ShopApi.Infrastructure.Messaging.Saga;

namespace ShopApi.Infrastructure.Messaging.Consumers
{
    public class OrderInventoryReservedConsumer
        : IConsumer<InventoryReserved>
    {
        private readonly ShopDbContext _context;
        private readonly IPublishEndpoint _publish;
        public OrderInventoryReservedConsumer(IPublishEndpoint publish, ShopDbContext context)
        {
            _publish = publish;
            _context = context;
        }


        public async Task Consume(
            ConsumeContext<InventoryReserved> context)
        {

            // پیدا کردن OrderSagaState

            //saga state
            var massage = context.Message;
            var saga = new OrderSagaState
            {
                CorrelationId = massage.CorrelationId,
                OrderId = massage.OrderId,
                CurrentState = "payment pending",
                CreatedAt = DateTime.UtcNow
            };
            _context.OrderSagaStates.Add(saga);
            await _context.SaveChangesAsync();

            var order = await _context.Orders
                .FirstOrDefaultAsync(
                    x => x.Id == massage.OrderId);

            if (order is null)
                return;

            order.Status = "InventoryReserved";
            await _context.SaveChangesAsync();



            await context.Send(
                new Uri("queue:request-payment"),
                new RequestPayment(
                    order.Id,
                    massage.CorrelationId,
                    order.TotalPrice));
        }
    }
}
