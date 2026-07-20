namespace ShopApi.Infrastructure.Services.Security
{
    public class PasswordHasher:IPasswordHasher
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        //check (input pass hash)=hashpass
        public bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    
    }
}
