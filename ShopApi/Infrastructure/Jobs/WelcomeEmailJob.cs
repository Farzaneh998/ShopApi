namespace ShopApi.Infrastructure.Jobs
{
    public class WelcomeEmailJob
    {
        private readonly ILogger<WelcomeEmailJob> _logger;

        public WelcomeEmailJob(ILogger<WelcomeEmailJob> logger)
        {
            _logger = logger;
        }

        public async Task SendWelcomeEmail(string email)
        {
            _logger.LogInformation(
                "Sending email to {Email}", email);

            await Task.Delay(5000);

            _logger.LogInformation(
                "Email sent.");
        }
    }
}
