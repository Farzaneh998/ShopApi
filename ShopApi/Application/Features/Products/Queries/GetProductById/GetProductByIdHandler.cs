using MediatR;
using Microsoft.EntityFrameworkCore;
using ShopApi.Infrastructure.Data;

namespace ShopApi.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
    {
        private readonly ShopDbContext _context;

        public GetProductByIdHandler(
            ShopDbContext context)
        {
            _context = context;
        }

        public async Task<ProductDto?> Handle(
          GetProductByIdQuery request,
          CancellationToken cancellationToken)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .Select(x => new ProductDto
                {
                    Id = x.Id,

                    Name = x.Name,

                    Price = x.Price
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
