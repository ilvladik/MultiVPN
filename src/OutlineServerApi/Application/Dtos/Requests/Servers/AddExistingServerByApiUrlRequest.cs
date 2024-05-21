namespace OutlineServerApi.Application.Dtos.Requests.Servers
{
    public sealed class AddExistingServerByApiUrlRequest
    {
        public required string Name { get; init; }

        public required string ApiUrl { get; init; }

        public Guid? CountryId { get; init; } = Guid.Empty;
    }
}
