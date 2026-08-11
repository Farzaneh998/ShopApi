namespace ShopApi.Contracts.Commands
{
    //public record ReserveInventory(
    //    int ProductId,
    //    int Quantity);

    public record ReserveInventory(
    int OrderId,
    Guid CorrelationId);
}
