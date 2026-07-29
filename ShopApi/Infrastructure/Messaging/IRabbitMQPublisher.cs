namespace ShopApi.Infrastructure.Messaging
{
    public interface IRabbitMQPublisher
    {
        Task Publish<T>(T message);
    }
}
