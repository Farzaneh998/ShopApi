namespace ShopApi.Contracts.Events
{
    public record OrderCreated(
        Guid CorrelationId,
        int OrderId);
}
