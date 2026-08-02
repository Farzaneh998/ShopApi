using MassTransit;
using ShopApi.Infrastructure.Messaging.Consumers;

namespace ShopApi.Infrastructure.Messaging
{
    public static class MassTransitConfiguration
    {
        public static IServiceCollection AddMassTransitConfiguration(
            this IServiceCollection services)//extention func(>program.cs)
        {

            services.AddMassTransit(x =>
            {
                x.AddConsumer<ProductCreatedConsumer>();
                x.AddConsumer<EmailConsumer>();
                x.AddConsumer<AuditConsumer>();



                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    //Retry
                    cfg.UseMessageRetry(r =>
                    {
                        r.Interval(3, TimeSpan.FromSeconds(5));
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
