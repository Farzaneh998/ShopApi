namespace ShopApi.Application.Features.Auth.Commands.Login
{
    public class TokenResult
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }
}
