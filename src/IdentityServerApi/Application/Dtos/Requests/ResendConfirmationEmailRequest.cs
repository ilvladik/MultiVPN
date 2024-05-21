namespace IdentityServerApi.Application.Dtos.Requests
{
    public class ResendConfirmationEmailRequest
    {
        public required string Email { get; init; }
    }
}
