using MediatR;

namespace ShopApi.Application.Features.Products.Events
{
    public class ProductCreatedCacheHandler:INotificationHandler<ProductCreatedEvent>
    {
        public async Task Handle(ProductCreatedEvent notification , CancellationToken cancellationToken)
        {
            Console.WriteLine("removed catch");
              
        }
    }
}
