using MediatR;

namespace ShopApi.Application.Features.Products.Queries.GetProductById
{
    public record GetProductByIdQuery(int Id):IRequest<ProductDto>;
    
    
}
