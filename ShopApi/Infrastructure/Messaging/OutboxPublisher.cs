using MassTransit;
using Microsoft.EntityFrameworkCore;
using ShopApi.Contracts.Events;
using ShopApi.Infrastructure.Data;
using System.Text.Json;

namespace ShopApi.Infrastructure.Messaging
{
    public class OutboxPublisher : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public OutboxPublisher(
            IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;

        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var db = scope.ServiceProvider
                    .GetRequiredService<ShopDbContext>();
                var publish = scope.ServiceProvider
                 .GetRequiredService<IPublishEndpoint>();

                var messages = await db.OutboxMessages
                    .Where(x => !x.Published)
                    .ToListAsync();

                foreach (var item in messages)
                {
                    if (item.Type == nameof(ProductCreated))
                    {
                        var evt =
                            JsonSerializer.Deserialize<ProductCreated>(
                                item.Payload);

                        await publish.Publish(evt!);

                        item.Published = true;
                    }
                }
                await db.SaveChangesAsync();


                await Task.Delay(
                    5000,
                    stoppingToken);
            }
        }
    }
}
