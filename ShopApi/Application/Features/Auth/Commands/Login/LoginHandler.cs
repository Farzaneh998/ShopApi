using MediatR;
using Microsoft.EntityFrameworkCore;
using ShopApi.Application.Common.Interfaces;
using ShopApi.Application.Features.Auth.Commands.Login;
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
                throw new Exception("Username یا Password اشتباه است.");
            if (user.Password != request.Password)
                throw new Exception("Username یا Password اشتباه است.");
            return new LoginResponse
            {
                Token = _tokenService.GenerateToken(user)
            };
        }
    }
}
