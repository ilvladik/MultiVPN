namespace IdentityServerApi.Core.Models
{
    public class RefreshToken
    {
        public RefreshToken() { }

        public RefreshToken(string value, DateTime expiresIn, User user)
        {
            Id = Guid.NewGuid();
            Value = value;
            ExpiresIn = expiresIn;
            UserId = user.Id;
            User = user;
        }
        public Guid Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public DateTime ExpiresIn { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
