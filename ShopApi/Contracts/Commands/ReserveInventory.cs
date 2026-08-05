namespace ShopApi.Contracts.Commands
{
    public record ReserveInventory(
        int ProductId,
        int Quantity);
}
