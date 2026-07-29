namespace ShopApi.Infrastructure.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

public class RabbitMQConsumer : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //connection
        Console.WriteLine("Consumer Started");
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };

        var connection =
            await factory.CreateConnectionAsync();

        //channel
        var channel =
            await connection.CreateChannelAsync();

        //dead letter ex
        await channel.ExchangeDeclareAsync(
    exchange: "product-dlx",
    type: ExchangeType.Direct,
    durable: true);


        //queue
        await channel.QueueDeclareAsync(
            queue: "product-created",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: new Dictionary<string, object?>
            {
                ["x-dead-letter-exchange"] = "product-dlx",
                ["x-dead-letter-routing-key"] = "dead"
            });
        var consumer =
            new AsyncEventingBasicConsumer(channel);

        //dead queue
        await channel.QueueDeclareAsync(
    queue: "product-created-dead",
    durable: true,
    exclusive: false,
    autoDelete: false);

        //bind
        await channel.QueueBindAsync(
    queue: "product-created-dead",
    exchange: "product-dlx",
    routingKey: "dead");

        //recive massgae
        consumer.ReceivedAsync += async (sender, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                // Console.WriteLine(json);
                // Console.WriteLine("Message Received");
                throw new Exception("Test Exception");

                //basicack
                await channel.BasicAckAsync(
                    deliveryTag: ea.DeliveryTag,
                    multiple: false);
            }
            catch (Exception ex)
            {
                //basicnack
                await channel.BasicNackAsync(
    deliveryTag: ea.DeliveryTag,
    multiple: false,
    requeue: false);// retry or dead letter

                // Console.WriteLine("NACK");
            }
        };

        await channel.BasicConsumeAsync(
    queue: "product-created",
    autoAck: false,
    consumer: consumer);

        await Task.Delay(
            Timeout.Infinite,
            stoppingToken);
    }
}