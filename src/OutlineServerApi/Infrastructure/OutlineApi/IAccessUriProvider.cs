namespace OutlineServerApi.Infrastructure.OutlineApi
{
    public interface IAccessUriProvider
    {
        Uri GetAccessUri(Guid id);
    }
}
