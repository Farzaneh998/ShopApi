using MediatR;

namespace ShopApi.Application.Features.Orders.Commands
{
    public record CreateOrderCommand(
        decimal TotalPrice
    ) : IRequest<int>;

}
