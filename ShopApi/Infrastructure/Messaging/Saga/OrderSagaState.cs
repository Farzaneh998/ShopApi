using MassTransit;

namespace ShopApi.Infrastructure.Messaging.Saga
{
    public class OrderSagaState: SagaStateMachineInstance
    {


        public Guid CorrelationId { get; set; }

        public int OrderId { get; set; }

        public string CurrentState { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime? CompletedAt { get; set; }
        public decimal Amount { get; set; }//for request payment step

    }
}
