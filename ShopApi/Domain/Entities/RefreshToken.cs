namespace ShopApi.Domain.Entities
{
    public class RefreshToken
    {
        public int Id { get; set; }

        public string Token { get; set; } = string.Empty;

        public DateTime Expires { get; set; }

        public DateTime Created { get; set; }

        //باطل
        public DateTime? Revoked { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public bool IsExpired
            => DateTime.UtcNow >= Expires;

        public bool IsActive
            => Revoked == null && !IsExpired;

    }
}
