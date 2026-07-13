using MediatR;

namespace ShopApi.Application.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand(

     string Name,
    decimal Price
) : IRequest<int>;

}
