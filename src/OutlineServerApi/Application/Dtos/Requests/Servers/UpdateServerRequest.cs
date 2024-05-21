namespace OutlineServerApi.Application.Dtos.Requests.Servers
{
    public sealed class UpdateServerRequest
    {
        public required string Name { get; init; }

        public required bool IsAvailable { get; init; }
    }
}
