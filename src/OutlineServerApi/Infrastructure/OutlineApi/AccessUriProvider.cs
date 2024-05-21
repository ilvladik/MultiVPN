namespace OutlineServerApi.Infrastructure.OutlineApi
{
    internal class AccessUriProvider : IAccessUriProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccessUriProvider(IHttpContextAccessor httpContextAccessor) =>
            _httpContextAccessor = httpContextAccessor;

        public Uri GetAccessUri(Guid id)
        {
            string host = _httpContextAccessor.HttpContext?.Request.Host.Host
                ?? throw new InvalidOperationException();
            int port = _httpContextAccessor.HttpContext?.Request.Host.Port
                ?? throw new InvalidOperationException();

            return new UriBuilder("ssconf", host, port, $"api/v1/keys/connect/{id}").Uri;
        }
    }
}
