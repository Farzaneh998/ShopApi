namespace ShopApi.Contracts.Events
{
    public record PaymentFailed(
        Guid CorrelationId,
        int OrderId,
        string Reason);
}
