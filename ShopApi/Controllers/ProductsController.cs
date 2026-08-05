using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Application.DTOs;
using ShopApi.Application.Features.Products.Commands.CreateProduct;
using ShopApi.Application.Features.Products.Queries.GetAllProducts;
using ShopApi.Application.Features.Products.Queries.GetProductById;
using ShopApi.Application.Services;
using ShopApi.Contracts.Commands;
using ShopApi.Contracts.Requests;
using ShopApi.Contracts.Responses;
using ShopApi.Domain.Entities;
using ShopApi.Exceptions;


namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    // [Authorize(Roles = "Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ProductService _productService;

        private readonly IMediator _mediator;
        private readonly IRequestClient<CheckInventoryRequest> _client;//request rabbitMQ
        private readonly ISendEndpointProvider _send;//send rabbit
        public ProductsController(ILogger<ProductsController> logger, ProductService productService, IMediator mediator,
            IRequestClient<CheckInventoryRequest> client, ISendEndpointProvider sendEndpointProvider)
        {
            _logger = logger;
            _productService = productService;
            _mediator = mediator;
            _client = client;
            _send = sendEndpointProvider;

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
        [Authorize(Policy = "Product.Create")]
        [HttpPost]
        public async Task<IActionResult> Create(
    CreateProductCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }


        [Authorize(Policy = "Product.Read")]
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




        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(
                    new GetAllProductQuery());

            return Ok(result);
        }

        //response/req(rabbit)
        [HttpGet("inventory")]
        public async Task<IActionResult> Inventory()
        {
            var response =
                await _client.GetResponse<
                    CheckInventoryResponse>(

                    new CheckInventoryRequest(5));

            return Ok(response.Message);
        }

        //send(rabit)
        [HttpPost("send")]
        public async Task<IActionResult> Send()
        {
            var endpoint = await _send.GetSendEndpoint(
                    new Uri("queue:reserve-inventory"));

            await endpoint.Send(
                new ReserveInventory(10, 2));
            return Ok();
        }
        #endregion
    }
}
