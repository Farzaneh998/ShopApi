namespace ShopApi.Infrastructure.Messaging;
using RabbitMQ.Client;
using ShopApi.Infrastructure.Messaging;
using System.Text;
using System.Text.Json;

public class RabbitMQPublisher : IRabbitMQPublisher
{
    public async Task Publish<T>(T message)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };


        await using var connection =
            await factory.CreateConnectionAsync();


        await using var channel =
            await connection.CreateChannelAsync();


        await channel.QueueDeclareAsync(
            queue: "product-created",
            durable: true,
            exclusive: false,
            autoDelete: false
        );


        var json = JsonSerializer.Serialize(message);

        var body = Encoding.UTF8.GetBytes(json);


        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: "product-created",
            body: body
        );
    }
}