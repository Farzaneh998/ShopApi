using MediatR;
using ShopApi.Application.Features.Products.Events;
using ShopApi.Domain.Entities;
using ShopApi.Infrastructure.Data;

namespace ShopApi.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly ShopDbContext _context;
        private readonly IMediator _mediator;

        public CreateProductHandler(
            ShopDbContext context, IMediator mediator)
        {
            _context = context;
            _mediator = mediator;
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

            //publish event
            await _mediator.Publish(
    new ProductCreatedEvent(
        product.Id,
        product.Name),
    cancellationToken);


            return product.Id;
        }


    }
}
