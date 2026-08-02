namespace ShopApi.Contracts.Responses
{
    public record CheckInventoryResponse(
        bool Exists,
        int Quantity);
}
