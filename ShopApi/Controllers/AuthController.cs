using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Application.Features.Auth.Commands.Login;
using ShopApi.Application.Features.Auth.Commands.RefreshToken;

namespace ShopApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);  
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenCommand command) 
        {
            var Result=await _mediator.Send(command);
            return Ok(Result);
        }



    }
}
