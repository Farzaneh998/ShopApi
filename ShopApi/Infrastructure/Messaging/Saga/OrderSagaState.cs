namespace ShopApi.Infrastructure.Messaging.Saga
{
    public class OrderSagaState
    {
        public int Id { get; set; }

        public Guid CorrelationId { get; set; }

        public int OrderId { get; set; }

        public string CurrentState { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime? CompletedAt { get; set; }
    }
}
