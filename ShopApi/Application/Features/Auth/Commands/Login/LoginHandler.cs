using MediatR;
using Microsoft.EntityFrameworkCore;
using ShopApi.Application.Common.Interfaces;
using ShopApi.Application.Features.Auth.Commands.Login;
using ShopApi.Domain.Entities;
using ShopApi.Infrastructure.Data;

namespace ShopApi.Application.Features.Auth.Commands
{
    public class LoginHandler:IRequestHandler<LoginCommand,LoginResponse>
    {
        private readonly ShopDbContext _context;
        private readonly ITokenService _tokenService;
        public LoginHandler(ShopDbContext context,ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user =await _context.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);
            if (user == null)
                throw new Exception(" User پیدا نشد.");
            if (user.Password != request.Password)
                throw new Exception("Username یا Password اشتباه است.");

            var tokens = _tokenService.GenerateTokens(user);

            //save
            var refreshToken = new ShopApi.Domain.Entities.RefreshToken
            {
                Token = tokens.RefreshToken,
                UserId = user.Id,
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(refreshToken);
            //user.RefreshTokens.Add(refreshToken);//redf collection
            await _context.SaveChangesAsync(cancellationToken);


            return new LoginResponse
            {
                AccessToken = tokens.AccessToken,

                RefreshToken = tokens.RefreshToken
            };
        }
    }
}
