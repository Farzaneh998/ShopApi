namespace ShopApi.Contracts.Events
{
    public record PaymentCompleted(
        Guid CorrelationId,
        int OrderId);
}
