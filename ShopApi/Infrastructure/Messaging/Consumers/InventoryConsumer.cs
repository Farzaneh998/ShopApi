using MassTransit;
using ShopApi.Contracts.Requests;
using ShopApi.Contracts.Responses;

namespace ShopApi.Infrastructure.Messaging.Consumers
{
    public class InventoryConsumer : IConsumer<CheckInventoryRequest>
    {
        public async Task Consume(ConsumeContext<CheckInventoryRequest> context)
        {
            await context.RespondAsync(
                new CheckInventoryResponse(

                    true,15));
        }
    }
}
