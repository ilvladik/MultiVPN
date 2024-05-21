namespace OutlineServerApi.Application.Dtos.Responses.Servers
{
    public sealed class ServerResponse
    {
        public required Guid Id { get; init; }

        public required string Name { get; init; }

        public required string ApiUrl { get; init; }

        public required string ServerAddress { get; init; }

        public required string Country { get; init; }

        public required int KeysCount { get; init; }

        public required bool IsAvailable { get; init; }
    }
}
