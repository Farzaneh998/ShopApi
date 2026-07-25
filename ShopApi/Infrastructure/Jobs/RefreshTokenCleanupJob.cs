using ShopApi.Infrastructure.Data;

namespace ShopApi.Infrastructure.Jobs
{
    public class RefreshTokenCleanupJob
    {
        private readonly ShopDbContext _context;

        public RefreshTokenCleanupJob(
            ShopDbContext context)
        {
            _context = context;
        }

        public async Task Execute()
        {
            var expired =_context.RefreshTokens
                    .Where(x => x.Expires < DateTime.UtcNow);

            _context.RefreshTokens.RemoveRange(expired);

            await _context.SaveChangesAsync();

            Console.WriteLine("Expired Tokens Deleted");
        }
    }
}