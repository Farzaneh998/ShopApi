using MediatR;

namespace ShopApi.Application.Features.Auth.Commands.Login
{
    public record LoginCommand(
    string UserName,
    string Password
) : IRequest<LoginResponse>;

}
