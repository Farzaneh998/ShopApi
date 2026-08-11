namespace ShopApi.Contracts.Commands
{
    public record RequestPayment(
        int OrderId,
        Guid CorrelationId,
        decimal Amount);
}
