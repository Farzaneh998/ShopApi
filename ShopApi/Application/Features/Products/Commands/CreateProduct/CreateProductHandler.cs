using MassTransit;
using MediatR;
using ShopApi.Application.Features.Products.Events;
using ShopApi.Contracts.Events;
using ShopApi.Domain.Entities;
using ShopApi.Infrastructure.Data;
using ShopApi.Infrastructure.Services.Caching;
using System.Text.Json;

namespace ShopApi.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly ShopDbContext _context;
        private readonly IMediator _mediator;
        private readonly ICacheService _cacheService;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateProductHandler(
            ShopDbContext context, IMediator mediator, ICacheService cacheService, IPublishEndpoint publishEndpoint)
        {
            _context = context;
            _mediator = mediator;
            _cacheService = cacheService;
            _publishEndpoint = publishEndpoint;

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

            //Masstransit pub
            //await _publishEndpoint.Publish(
            //    new ProductCreated(
            //        product.Id,
            //        product.Name,
            //        product.Price));


            #region(outbox pattern)
            var evt = new ProductCreated(
    product.Id,
    product.Name,
    product.Price);

            _context.OutboxMessages.Add(
                new OutboxMessage
                {
                    Type = nameof(ProductCreated),
                    Payload = JsonSerializer.Serialize(evt),
                    CreatedAt = DateTime.UtcNow,
                    Published = false
                });

            await _context.SaveChangesAsync();
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
