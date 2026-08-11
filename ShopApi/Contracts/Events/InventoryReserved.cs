namespace ShopApi.Contracts.Events
{
    public record InventoryReserved(
     Guid CorrelationId,
     int OrderId);

}
