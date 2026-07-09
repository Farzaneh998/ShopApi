using Microsoft.AspNetCore.Mvc;

namespace ShopApi.Controllers
{
    public class testlogController : Controller
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (id == 0)
                throw new Exception("Database error");

            return Ok(new
            {
                Id = id,
                Name = "Laptop"
            });
        }
    }
}
