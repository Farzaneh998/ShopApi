using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Application.DTOs;
using ShopApi.Application.Features.Products.Commands.CreateProduct;
using ShopApi.Application.Features.Products.Queries.GetProductById;
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

        private readonly IMediator _mediator;

        public ProductsController(ILogger<ProductsController> logger, ProductService productService, IMediator mediator)
        {
            _logger = logger;
            _productService = productService;
            _mediator = mediator;

        }


        //    #region(callservice)
        //    [HttpPost]
        //    public async Task<IActionResult> Create(CreateProductDto product, CancellationToken cancellationToken)
        //    {
        //        var result =
        //            await _productService.CreateAsync(product, cancellationToken);
        //        return Ok(result);
        //    }


        //    [HttpGet("{id}")]
        //    public async Task<IActionResult> Get(
        //int id,
        //CancellationToken cancellationToken)
        //    {
        //        var product =
        //            await _productService.GetByIdAsync(
        //                id,
        //                cancellationToken);

        //        if (product is null)
        //            return NotFound();

        //        return Ok(product);
        //    }


        //    [HttpGet]
        //    public async Task<IActionResult> GetAll(
        //[FromQuery] ProductQueryDto query,
        //CancellationToken cancellationToken)
        //    {
        //        var result =
        //            await _productService.GetAllAsync(query, cancellationToken);

        //        return Ok(result);
        //    }

        //    #endregion


        #region(cqrs)

        [HttpPost]
        public async Task<IActionResult> Create(
    CreateProductCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id,
           CancellationToken cancellationToken)
        {
            var result =
                await _mediator.Send(
                    new GetProductByIdQuery(id),
                    cancellationToken);

            if (result is null)
                return NotFound();

            return Ok(result);
        }

        #endregion
    }
}
