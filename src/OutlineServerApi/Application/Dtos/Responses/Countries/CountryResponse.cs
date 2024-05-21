namespace OutlineServerApi.Application.Dtos.Responses.Countries
{
    public sealed class CountryResponse
    {
        public required Guid Id { get; init; }

        public required string Name { get; init; }
    }
}
