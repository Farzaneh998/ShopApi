using MassTransit;
using ShopApi.Infrastructure.Data;
using ShopApi.Infrastructure.Messaging.Consumers;
using ShopApi.Infrastructure.Messaging.Saga;
using System;
using Microsoft.EntityFrameworkCore;


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


               // x.AddConsumer<OrderCreatedConsumer>();
                x.AddConsumer<ReserveInventoryConsumer>();
               // x.AddConsumer<OrderInventoryReservedConsumer>();
                x.AddConsumer<PaymentConsumer>();
              //  x.AddConsumer<PaymentCompletedConsumer>();
                //x.AddConsumer<PaymentFailedConsumer>();


                //outbox patt
                //x.AddEntityFrameworkOutbox<ShopDbContext>(o =>
                //{
                //    o.UseSqlServer();

                //    o.UseBusOutbox();
                //});

                //saga
                x.AddSagaStateMachine<OrderStateMachine, OrderSagaState>()
                    .EntityFrameworkRepository(r =>
                    {
                        r.ExistingDbContext<ShopDbContext>();
                        r.UseSqlServer();
                    });


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


                    //send command(qeue)
                    cfg.ReceiveEndpoint("reserve-inventory", e =>
                    {
                        e.ConfigureConsumer<ReserveInventoryConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("request-payment", e =>
                    {
                        e.ConfigureConsumer<PaymentConsumer>(context);
                    });


                    cfg.ConfigureEndpoints(context);
                });

            });
            return services;
        }
    }
}
