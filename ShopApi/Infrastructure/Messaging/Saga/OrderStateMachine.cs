using MassTransit;
using ShopApi.Contracts.Commands;
using ShopApi.Contracts.Events;

namespace ShopApi.Infrastructure.Messaging.Saga
{
    public class OrderStateMachine
        : MassTransitStateMachine<OrderSagaState>

    {
        #region(State)
        public State InventoryReserving { get; private set; }
        public State PaymentProcessing { get; private set; }
        public State Completed { get; private set; }
        public State Cancelled { get; private set; }
        #endregion

        #region(Envent)
        public Event<OrderCreated> OrderCreated { get; private set; } = null!;
        public Event<InventoryReserved> InventoryReserved { get; private set; } = null!;
        public Event<PaymentCompleted> PaymentCompleted { get; private set; } = null!;
        public Event<PaymentFailed> PaymentFailed { get; private set; } = null!;
        #endregion

        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderCreated, x =>
            {
                x.CorrelateById(context => context.Message.CorrelationId);
            });

            Event(() => InventoryReserved, x =>
            {
                x.CorrelateById(context => context.Message.CorrelationId);
            });

            Event(() => PaymentCompleted, x =>
            {
                x.CorrelateById(context => context.Message.CorrelationId);
            });

            Event(() => PaymentFailed, x =>
            {
                x.CorrelateById(context => context.Message.CorrelationId);
            });


            Initially(
                When(OrderCreated)
                    .Then(context =>
                    {
                        //data
                        context.Saga.OrderId = context.Message.OrderId;
                        context.Saga.Amount = context.Message.Amount;
                    })
                    .Send(
                        new Uri("queue:reserve-inventory"),
                        context => new ReserveInventory(
                            context.Message.OrderId,
                            context.Message.CorrelationId))
                    .TransitionTo(InventoryReserving)
            );


            During(
                InventoryReserving,

                When(InventoryReserved)
                    .Send(
                        new Uri("queue:request-payment"),
                        context => new RequestPayment(
                            context.Message.OrderId,
                            context.Message.CorrelationId,
                            context.Saga.Amount))
                    .TransitionTo(PaymentProcessing)
            );


            During(
                PaymentProcessing,

                When(PaymentCompleted)
                    .TransitionTo(Completed)
                    .Finalize(),

                When(PaymentFailed)
                    .TransitionTo(Cancelled)
                    .Finalize()
            );

           // SetCompletedWhenFinalized();
        }






    }
}
