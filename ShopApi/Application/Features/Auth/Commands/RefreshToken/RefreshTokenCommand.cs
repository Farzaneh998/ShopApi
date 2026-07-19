using MediatR;
using ShopApi.Application.Features.Auth.Commands.Login;

namespace ShopApi.Application.Features.Auth.Commands.RefreshToken
{
    public record RefreshTokenCommand(
      string RefreshToken
  ) : IRequest<LoginResponse>;

}
