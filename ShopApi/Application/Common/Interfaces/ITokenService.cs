using ShopApi.Application.Features.Auth.Commands.Login;
using ShopApi.Domain.Entities;

namespace ShopApi.Application.Common.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);//jwt
  
        //jwt+refresh
         TokenResult GenerateTokens(User user);
      
    }
}
