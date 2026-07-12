using Microsoft.AspNetCore.Mvc;
using ShopApi.Application.DTOs;
using ShopApi.Application.Services;
using ShopApi.Domain.Entities;
using ShopApi.Exceptions;


namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/products")]

    public class ProductsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ProductService _productService;

        public ProductsController(ILogger<ProductsController> logger, ProductService productService)
        {
            _logger = logger;
            _productService = productService;


        }
        //[HttpGet("{id}")]
        //public IActionResult Get(int id)
        //{
        //    _logger.LogInformation("Controller executed");
        //    if (id == 0)
        //        throw new NotFoundException("Product not found");

        //    return Ok(new { Id = id, Name = "Laptop" });
        //}


        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto product, CancellationToken cancellationToken)
        {
            var result =
                await _productService.CreateAsync(product, cancellationToken);
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(
    int id,
    CancellationToken cancellationToken)
        {
            var product =
                await _productService.GetByIdAsync(
                    id,
                    cancellationToken);

            if (product is null)
                return NotFound();

            return Ok(product);
        }


//        [HttpGet]
//        public async Task<IActionResult> GetAll(
//CancellationToken cancellationToken)
//        {
//            var result =
//               await _productService.GetAllAsync(cancellationToken);

//            return Ok(result);
//        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
    [FromQuery] ProductQueryDto query,
    CancellationToken cancellationToken)
        {
            var result =
                await _productService.GetAllAsync(query, cancellationToken);

            return Ok(result);
        }
    }
}
