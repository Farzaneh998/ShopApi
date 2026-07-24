using MediatR;
using ShopApi.Application.Features.Products.Queries.GetProductById;

namespace ShopApi.Application.Features.Products.Queries.GetAllProducts
{
    public record GetAllProductQuery() : IRequest<List<ProductDto>>;
  
}
