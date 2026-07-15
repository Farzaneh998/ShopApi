using ShopApi.Domain.Entities;

namespace ShopApi.Application.Common.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
