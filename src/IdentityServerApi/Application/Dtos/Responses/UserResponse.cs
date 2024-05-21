namespace IdentityServerApi.Application.Dtos.Responses
{
    public class UserResponse
    {
        public required string Id { get; set; }

        public required string Email { get; set; }

        public required string[] Roles { get; set; }
    }
}
