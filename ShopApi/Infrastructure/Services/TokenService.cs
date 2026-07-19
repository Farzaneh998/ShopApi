using Microsoft.IdentityModel.Tokens;
using ShopApi.Application.Common.Interfaces;
using ShopApi.Application.Features.Auth.Commands.Login;
using ShopApi.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ShopApi.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //AccsessToken(jwt)
        public string GenerateToken(User user)
        {
            // Claims
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Name,user.UserName),
            new Claim(ClaimTypes.Role,user.Role)
        };

            //Secret Key from appsetting 
            var key = _configuration["Jwt:Key"]!;

            var securityKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key));

            //Sign
            var credentials =
                new SigningCredentials(
                    securityKey,
                    SecurityAlgorithms.HmacSha256);

            // Create Token
            var token =
                new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(
                        Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                    signingCredentials: credentials);

            //Convert To String
            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        //داخلی
        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        public TokenResult GenerateTokens(User user)
        {
            return new TokenResult
            {
                AccessToken = GenerateToken(user),

                RefreshToken = GenerateRefreshToken()
            };
        }

    }
}