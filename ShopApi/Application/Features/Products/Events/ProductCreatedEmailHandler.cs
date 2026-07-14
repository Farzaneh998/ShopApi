using MediatR;

namespace ShopApi.Application.Features.Products.Events
{
    public class ProductCreatedEmailHandler
     : INotificationHandler<ProductCreatedEvent>
    {
        public async Task Handle(
            ProductCreatedEvent notification,
            CancellationToken cancellationToken)
        {
            Console.WriteLine(
                $"Email : {notification.Name}");
        }
    }
}