namespace IdentityServerApi.Infrastructure.Jwt.Models
{
    public sealed class Token
    {
        public required string Value { get; init; }

        public required DateTime ExpiresIn { get; init; }
    }
}
