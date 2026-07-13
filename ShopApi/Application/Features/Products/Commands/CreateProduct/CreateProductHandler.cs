using MediatR;
using ShopApi.Domain.Entities;
using ShopApi.Infrastructure.Data;

namespace ShopApi.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly ShopDbContext _context;

        public CreateProductHandler(
            ShopDbContext context)
        {
            _context = context;
        }


        public async Task<int> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = request.Name,

                Price = request.Price,

                CreatedAt = DateTime.UtcNow
            };

            await _context.Products
                .AddAsync(product, cancellationToken);

            await _context.SaveChangesAsync(
                cancellationToken);

            return product.Id;
        }


    }
}
