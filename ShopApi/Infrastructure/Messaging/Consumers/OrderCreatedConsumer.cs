using MassTransit;
using ShopApi.Contracts.Commands;
using ShopApi.Contracts.Events;
using ShopApi.Infrastructure.Data;
using ShopApi.Infrastructure.Messaging.Saga;

namespace ShopApi.Infrastructure.Messaging.Consumers
{
    public class OrderCreatedConsumer:IConsumer<OrderCreated>
    {
        private readonly ShopDbContext _context;
        private readonly IPublishEndpoint _publish;
        public OrderCreatedConsumer(IPublishEndpoint publish,ShopDbContext context )
        {
            _publish = publish;
            _context = context;
        }

        public async Task Consume (ConsumeContext<OrderCreated> context)
        {
            //saga state
            var massage = context.Message;
            var saga = new OrderSagaState
            {
                CorrelationId = massage.CorrelationId,
                OrderId = massage.OrderId,
                CurrentState = "InventoryPending",
                CreatedAt = DateTime.UtcNow
            };
            _context.OrderSagaStates.Add( saga );
            await  _context.SaveChangesAsync();

            // reservcommand
            await context.Send(
              new Uri("queue:reserve-inventory"),
              new ReserveInventory(
                  massage.OrderId,
                  massage.CorrelationId));
        }
    }
}
