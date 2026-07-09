using Microsoft.AspNetCore.Mvc;
using ShopApi.Exceptions;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/products")]

    public class ProductsController : ControllerBase
    {
        private readonly ILogger _logger;
        public ProductsController (ILogger<ProductsController> logger)
        {
            _logger = logger;
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            _logger.LogInformation("Controller executed");
            if (id == 0)
                throw new NotFoundException("Product not found");

            return Ok(new { Id = id, Name = "Laptop" });
        }
    }
}
