using MediatR;
using Microsoft.EntityFrameworkCore;
using ShopApi.Application.Common.Interfaces;
using ShopApi.Application.Features.Auth.Commands.Login;
using ShopApi.Infrastructure.Data;
using ShopApi.Domain.Entities;

namespace ShopApi.Application.Features.Auth.Commands.RefreshToken
{
    public class RefreshTokenHandler
        : IRequestHandler<RefreshTokenCommand, LoginResponse>
    {

        private readonly ShopDbContext _context;
        private readonly ITokenService _tokenService;


        public RefreshTokenHandler(
            ShopDbContext context,
            ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {

            var refreshToken =
                await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(
                    x => x.Token == request.RefreshToken,
                    cancellationToken);


            if (refreshToken == null)
                throw new Exception("Refresh token invalid");


            if (refreshToken.Expires < DateTime.UtcNow)
                throw new Exception("Refresh token expired");


            if (refreshToken.Revoked != null)
                throw new Exception("Refresh token revoked");


            // revoke old token
            refreshToken.Revoked = DateTime.UtcNow;

            // save refresh
            var tokens =
                _tokenService.GenerateTokens(refreshToken.User);


            var newRefreshToken = new ShopApi.Domain.Entities.RefreshToken
            {
                Token = tokens.RefreshToken,
                UserId = refreshToken.UserId,
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddDays(7)
            };


            _context.RefreshTokens.Add(newRefreshToken);
            await _context.SaveChangesAsync(cancellationToken);

            return new LoginResponse
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken
            };

        }
    }
}
