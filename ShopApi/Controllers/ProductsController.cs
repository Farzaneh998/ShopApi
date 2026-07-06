using Microsoft.AspNetCore.Mvc;
using ShopApi.Exceptions;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/products")]

    public class ProductsController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id == 0)
                throw new NotFoundException("Product not found");

            return Ok(new { Id = id, Name = "Laptop" });
        }
    }
}
