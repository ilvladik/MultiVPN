namespace IdentityServerApi.Application.Dtos.Responses
{
    public sealed class AccessTokenResponse
    {
        public required string AccessToken { get; init; }

        public required long ExpiresIn {  get; init; }

        public required string RefreshToken { get; init; }


    }
}
