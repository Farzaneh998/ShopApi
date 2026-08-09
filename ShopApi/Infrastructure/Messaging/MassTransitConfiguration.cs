using MassTransit;
using ShopApi.Infrastructure.Messaging.Consumers;
using ShopApi.Infrastructure.Data;
using System;

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
                x.AddConsumer<InventoryConsumer>();
                x.AddConsumer<ReserveInventoryConsumer>();



                //outbox patt
                //x.AddEntityFrameworkOutbox<ShopDbContext>(o =>
                //{
                //    o.UseSqlServer();

                //    o.UseBusOutbox();
                //});


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


                    //send command
                    cfg.ReceiveEndpoint("reserve-inventory", e =>
                    {
                        e.ConfigureConsumer<ReserveInventoryConsumer>(context);
                    });

                    cfg.ConfigureEndpoints(context);
                });

            });
            return services;
        }
    }
}
