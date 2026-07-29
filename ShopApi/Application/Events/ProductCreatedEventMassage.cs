namespace ShopApi.Application.Events
{
    public class ProductCreatedEventMassage
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
