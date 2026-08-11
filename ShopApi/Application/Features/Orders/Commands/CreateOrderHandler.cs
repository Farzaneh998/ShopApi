using MassTransit;
using MediatR;
using ShopApi.Contracts.Events;
using ShopApi.Domain.Entities;
using ShopApi.Infrastructure.Data;
using StackExchange.Redis;
using System.Threading;

namespace ShopApi.Application.Features.Orders.Commands
{
    public class CreateOrderHandler
        : IRequestHandler<CreateOrderCommand, int>
    {
        private readonly ShopDbContext _context;
        private readonly IPublishEndpoint _publish;

        public CreateOrderHandler(
            ShopDbContext context,
            IPublishEndpoint publish)
        {
            _context = context;
            _publish = publish;
        }

        public async Task<int> Handle(
            CreateOrderCommand request,
            CancellationToken cancellationToken)
        {
            var correlationId = Guid.NewGuid();

            var order = new Domain.Entities.Order
            {
                CorrelationId = correlationId,
                TotalPrice = request.TotalPrice,
                Status = "Created",
                CreatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);

            await _context.SaveChangesAsync(cancellationToken);

            await _publish.Publish(
                new OrderCreated(
                    correlationId,
                    order.Id),
                cancellationToken);

            return order.Id;
        }
    }
}
