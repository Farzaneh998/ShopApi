namespace ShopApi.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public Guid CorrelationId { get; set; }

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Created";

        public DateTime CreatedAt { get; set; }
    }
}
