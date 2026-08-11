namespace ShopApi.Contracts.Events
{
    public record InventoryReservationFailed(
        Guid CorrelationId,
        int OrderId,
        string Reason);
}
