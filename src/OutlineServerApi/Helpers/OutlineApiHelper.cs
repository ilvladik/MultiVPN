namespace OutlineServerApi.Helpers
{
    internal static class OutlineApiHelper
    {
        public static string GetHostname(string apiUrl) =>
            new UriBuilder(apiUrl).Host;

        public static int GetPort(string apiUrl) =>
            new UriBuilder(apiUrl).Port;

        public static string GetApiPrefix(string apiUrl) =>
            new UriBuilder(apiUrl).Path.Trim('/');

        public static Uri GetUri(string apiUrl) =>
            new UriBuilder(apiUrl).Uri;

        public static Uri GetUri(string hostname, int port, string apiPrefix) =>
            new UriBuilder(Uri.UriSchemeHttps, hostname, port, apiPrefix).Uri;

    }
}
