using MediatR;

namespace ShopApi.Application.Features.Products.Events
{
    public record ProductCreatedEvent(
     int ProductId,
     string Name
 ) : INotification;

}
