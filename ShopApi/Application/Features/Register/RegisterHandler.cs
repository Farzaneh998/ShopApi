using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopApi.Domain.Entities;
using ShopApi.Infrastructure.Data;
using ShopApi.Infrastructure.Services.Security;

namespace ShopApi.Application.Features.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand>
    {
        private readonly ShopDbContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public RegisterHandler(
            ShopDbContext context,
            IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _context.Users.AnyAsync(x => x.UserName == request.UserName))
                throw new Exception("Username exists");

            var user = new User
            {
                UserName = request.UserName,
                Password = _passwordHasher.Hash(request.Password),
                Role = "User"
            };

            _context.Users.Add(user);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
