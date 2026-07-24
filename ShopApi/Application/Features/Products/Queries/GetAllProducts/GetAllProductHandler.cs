using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using ShopApi.Application.Features.Products.Queries.GetProductById;
using ShopApi.Infrastructure.Data;
using ShopApi.Infrastructure.Services.Caching;
using System.Collections.Generic;
using System.Text.Json;

namespace ShopApi.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductHandler : IRequestHandler<GetAllProductQuery, List<ProductDto>>
    {
        private readonly ShopDbContext _context;
        // private readonly IDistributedCache _cache;
        private readonly ICacheService _cacheService;
        public GetAllProductHandler(
            ShopDbContext context, ICacheService cacheService
            //,IDistributedCache cache
            )
        {
            _context = context;
            _cacheService = cacheService;
            // _cache = cache;
        }

        public async Task<List<ProductDto>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            #region(redis in handle)
            //const string cacheKey = "products";
            //var cache = await _cache.GetStringAsync(cacheKey, cancellationToken);
            //if (cache != null)
            //{
            //   //* Console.WriteLine("FROM REDIS");
            //    return JsonSerializer.Deserialize<List<ProductDto>>(cache)!;
            //}

            //* Console.WriteLine("FROM SQL");
            //var products =
            //    await _context.Products
            //    .Select(x => new ProductDto
            //    {
            //        Id = x.Id,
            //        Name = x.Name,
            //        Price = x.Price
            //    })
            //    .ToListAsync(cancellationToken);

            //var json = JsonSerializer.Serialize(products);

            //await _cache.SetStringAsync(
            //    cacheKey,
            //    json,
            //    new DistributedCacheEntryOptions
            //    {
            //        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)//after off db
            //    },
            //    cancellationToken);

            //return products;
            #endregion

            return await _cacheService.GetOrCreateAsync("products",

     async () =>
     {
         return await _context.Products
             .Select(x => new ProductDto
             {
                 Id = x.Id,
                 Name = x.Name,
                 Price = x.Price
             })
             .ToListAsync(cancellationToken);
     },
     TimeSpan.FromMinutes(5));
        }
    }

}
