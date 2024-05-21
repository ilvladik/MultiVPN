namespace OutlineServerApi.Application.Dtos.Requests.Keys
{
    public sealed class TransferKeyToNewServerRequest
    {
        public Guid? ServerId { get; init; } = Guid.Empty;
    }
}
