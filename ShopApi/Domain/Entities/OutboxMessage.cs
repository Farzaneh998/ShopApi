namespace ShopApi.Domain.Entities
{
    public class OutboxMessage
    {
        public int Id { get; set; }

        public string Type { get; set; } = null!;

        public string Payload { get; set; } = null!;

        public bool Published { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
