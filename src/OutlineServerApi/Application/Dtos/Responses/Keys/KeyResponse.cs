namespace OutlineServerApi.Application.Dtos.Responses.Keys
{
    public sealed class KeyResponse
    {
        public required Guid Id { get; init; }

        public required string Name { get; init; }

        public required string ServerAddress { get; init; }

        public required Guid CreatedByUser { get; init; }

        public required string Country { get; init; }

        public required string AccessUri { get; init; }
    }

}
