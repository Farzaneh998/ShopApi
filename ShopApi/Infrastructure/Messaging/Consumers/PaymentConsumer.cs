using MassTransit;
using ShopApi.Contracts.Commands;
using ShopApi.Contracts.Events;

namespace ShopApi.Infrastructure.Messaging.Consumers
{
    public class PaymentConsumer
        : IConsumer<RequestPayment>
    {
        public async Task Consume(
            ConsumeContext<RequestPayment> context)
        {
            var message = context.Message;

            Console.WriteLine(
                $"Processing payment for Order: {message.OrderId}");

            //payment (درگاه بانکی)
            var paymentSucceeded = true;

            if (paymentSucceeded)
            {
                await context.Publish(
                    new PaymentCompleted(
                        message.CorrelationId,
                        message.OrderId
                        ));

                return;
            }

            await context.Publish(
                new PaymentFailed(
                     message.CorrelationId,
                    message.OrderId,
                    "Payment declined"));
        }
    }
}
