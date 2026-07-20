using MediatR;

namespace ShopApi.Application.Features.Register
{
    public record RegisterCommand(
        string UserName,
        string Password
    ) : IRequest;
}
