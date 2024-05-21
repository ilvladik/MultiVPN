namespace OutlineServerApi.Application.Dtos.Requests.Keys
{
    public sealed class TransferKeysRequest
    {
        public Guid? ServerId { get; init; } = Guid.Empty;
    }
}
