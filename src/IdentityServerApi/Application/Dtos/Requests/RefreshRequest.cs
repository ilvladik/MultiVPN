namespace IdentityServerApi.Application.Dtos.Requests
{
    public class RefreshRequest
    {
        public required string RefreshToken { get; init; }
    }
}
