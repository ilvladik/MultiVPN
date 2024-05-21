namespace OutlineServerApi.Application.Dtos.Requests.Keys
{
    public sealed class CreateNewKeyRequest
    {
        public required string Name { get; init; }

        public Guid? CountryId { get; init; } = Guid.Empty;
    }
}
