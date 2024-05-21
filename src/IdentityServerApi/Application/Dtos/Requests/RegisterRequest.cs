namespace IdentityServerApi.Application.Dtos.Requests
{
    public class RegisterRequest
    {
        public required string Email { get; init; }

        public required string Password { get; init; }
    }
}
