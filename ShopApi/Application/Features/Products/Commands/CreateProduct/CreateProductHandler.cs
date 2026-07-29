using MediatR;
using ShopApi.Application.Events;
using ShopApi.Application.Features.Products.Events;
using ShopApi.Domain.Entities;
using ShopApi.Infrastructure.Data;
using ShopApi.Infrastructure.Messaging;
using ShopApi.Infrastructure.Services.Caching;

namespace ShopApi.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly ShopDbContext _context;
        private readonly IMediator _mediator;
        private readonly ICacheService _cacheService;
        private readonly IRabbitMQPublisher _publisher;
        public CreateProductHandler(
            ShopDbContext context, IMediator mediator, ICacheService cacheService, IRabbitMQPublisher rabbitMQPublisher)
        {
            _context = context;
            _mediator = mediator;
            _cacheService = cacheService;
            _publisher = rabbitMQPublisher;
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

            #region(publish event RabitMQ)

            await _publisher.Publish(
    new ProductCreatedEventMassage
    {
        ProductId = product.Id,
        Name = product.Name,
        CreatedAt = DateTime.UtcNow
    });

            #endregion

            #region(publish event : DOMAIN Driven event)
            await _mediator.Publish(
    new ProductCreatedEvent(
        product.Id,
        product.Name),
    cancellationToken);

            #endregion
            //remove cache
          //  await _cacheService.RemoveAsync("products");


            return product.Id;
        }


    }
}
