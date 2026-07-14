using MediatR;

namespace ShopApi.Application.Features.Products.Events
{
    public class ProductCreatedAuditHandler
     : INotificationHandler<ProductCreatedEvent>
    {
        public async Task Handle(
            ProductCreatedEvent notification,
            CancellationToken cancellationToken)
        {
            Console.WriteLine("Audit Saved");
        }
    }
}
