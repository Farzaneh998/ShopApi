using Microsoft.EntityFrameworkCore;
using ShopApi.Application.Common.Models;
using ShopApi.Application.DTOs;
using ShopApi.Domain.Entities;
using ShopApi.Infrastructure.Data;
using System.Threading;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ShopApi.Application.Services
{
    public class ProductService
    {
        private readonly ShopDbContext _context;


        public ProductService(ShopDbContext context)
        {
            _context = context;
        }


        public async Task<Product> CreateAsync(CreateProductDto dto, CancellationToken cancellationToken)
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Products.AddAsync(product, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return product;
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);
        }

        public async Task<List<ProductListDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            //query
            return await _context.Products
                .AsNoTracking()
                .Select(x => new ProductListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price
                })
                .ToListAsync(cancellationToken);

            //var sql = query.ToQueryString();
            //Console.WriteLine(sql);  sql query ef
        }

        //        public async Task<List<ShowOrderListDto>> GetAllProductAsync(CancellationToken cancellationToken)
        //        {
        //            var orders = await _context.Orders
        //.Select(x => new ShowOrderListDto
        //{
        //    Id = x.Id,

        //    CustomerName = x.Customer.Name,

        //    Total = x.Items.Sum(i => i.Price)
        //})
        //.ToListAsync();
        //           //// var query = orders.ToQueryString();
        //            return orders;
        //        }




        public async Task<PagedResult<ProductListDto>> GetAllAsync(
            ProductQueryDto query,
            CancellationToken cancellationToken)
        {

            IQueryable<Product> products = _context.Products.AsNoTracking();

            // Filtering
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                products = products.Where(x =>
                    x.Name.Contains(query.Search));
            }

            var totalCount =
    await products.CountAsync(cancellationToken);

            // Sorting
            products = query.Sort switch
            {
                "price" => products.OrderBy(x => x.Price),

                "price_desc" => products.OrderByDescending(x => x.Price),

                "name" => products.OrderBy(x => x.Name),

                "name_desc" => products.OrderByDescending(x => x.Name),

                _ => products.OrderBy(x => x.Id)
            };

            // Pagination
            query.PageSize =
      Math.Clamp(query.PageSize, 1, 100);

            var items = await products
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)

                .Select(x => new ProductListDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price
                })
                .ToListAsync(cancellationToken);

            //projection
            return new PagedResult<ProductListDto>
            {
                Items = items,

                Page = query.Page,

                PageSize = query.PageSize,

                TotalCount = totalCount
            };
        }

    }
}
