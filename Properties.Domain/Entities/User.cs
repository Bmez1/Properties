using Crosscutting;

namespace Properties.Domain.Entities
{
    public class User : EntityBase
    {
        public string Email { get; private set; } 
        public string PasswordHash { get; private set; }

        private User(string email, string passwordHash, DateTime createdAt)
        {
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = createdAt;
        }

        public static User Create(string email, string passwordHash) => 
            new (email, passwordHash, DateTime.UtcNow); 
    }
}
