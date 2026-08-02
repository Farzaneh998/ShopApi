namespace ShopApi.Contracts.Events
{
    public record ProductCreated
    (
        int ProductId,
        string Name,
        decimal Price
    );
}
