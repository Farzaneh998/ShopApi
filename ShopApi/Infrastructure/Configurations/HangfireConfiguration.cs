using Hangfire;
using ShopApi.Infrastructure.Jobs;

namespace ShopApi.Infrastructure.Configurations
{
    public static class HangfireConfiguration
    {
        public static IServiceCollection AddHangfireConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(
                    configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddHangfireServer();

            return services;
        }

        public static WebApplication UseHangfireConfiguration(
            this WebApplication app)
        {
            app.UseHangfireDashboard();//pipline

            var jobs = app.Services.GetRequiredService<IRecurringJobManager>();

            //schedule job
            jobs.AddOrUpdate<RefreshTokenCleanupJob>(
                "cleanup-refresh",
                x => x.Execute(),
                Cron.Daily());


            //jobs.AddOrUpdate<InventorySyncJob>(
            //"inventory-sync",
            //x => x.Execute(),
            //Cron.Hourly());

            return app;
        }
    }
}
