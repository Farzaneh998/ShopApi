using MassTransit.Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Application.Features.Orders.Commands;
using ShopApi.Application.Features.Products.Commands.CreateProduct;
using MediatR;
using IMediator = MediatR.IMediator;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : Controller
    {
        private readonly MediatR.IMediator _mediator;

        public OrderController(IMediator mediator)
        {
         _mediator = mediator;       
        }


        [HttpPost]
        public async Task<IActionResult> Create(
            CreateOrderCommand command,
            CancellationToken cancellationToken)
        {
            var orderId = await  _mediator.Send(
                command,
                cancellationToken);

            return Ok(orderId);
        }
    }
}
