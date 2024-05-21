using OutlineServerApi.Infrastructure.Extensions;
using OutlineServerApi.Infrastructure.OutlineApi.Models;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace OutlineServerApi.Infrastructure.OutlineApi
{
    internal class OutlineApi : IOutlineApi
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public OutlineApi(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<OutlineKey> CreateKeyAsync(Uri serverAddress)
        {
            using Stream stream = await SendRequestAsync(HttpMethod.Post, serverAddress.Append("/access-keys"));
            var key = JsonSerializer.Deserialize<OutlineKey>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (key is not null)
                return key;

            throw new BadHttpRequestException("The request could not be performed. A null object was obtained after the deserialization process. Please check the request and the type of result mapping object");
        }

        public async Task<IEnumerable<OutlineKey>> GetAllKeysAsync(Uri serverAddress)
        {
            using Stream stream = await SendRequestAsync(HttpMethod.Get, serverAddress.Append("/access-keys"));
            var keys = JsonSerializer.Deserialize<OutlineKeys>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (keys is not null)
                return keys.AccessKeys;

            throw new BadHttpRequestException("The request could not be performed. A null object was obtained after the deserialization process. Please check the request and the type of result mapping object");
        }

        public async Task<OutlineKey> GetKeyByIdAsync(Uri serverAddress, string id)
        {
            using Stream stream = await SendRequestAsync(HttpMethod.Get, serverAddress.Append($"/access-keys/{id}"));
            var key = JsonSerializer.Deserialize<OutlineKey>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (key is not null)
                return key;

            throw new BadHttpRequestException("The request could not be performed. A null object was obtained after the deserialization process. Please check the request and the type of result mapping object");
        }

        public async Task<OutlineServer> GetServerAsync(Uri serverAddress)
        {
            using Stream stream = await SendRequestAsync(HttpMethod.Get, serverAddress.Append($"/server"));
            var server = JsonSerializer.Deserialize<OutlineServer>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (server is not null)
                return server;

            throw new BadHttpRequestException("The request could not be performed. A null object was obtained after the deserialization process. Please check the request and the type of result mapping object");
        }

        public async Task DeleteKeyAsync(Uri serverAddress, string id)
        {
            await SendRequestAsync(HttpMethod.Delete, serverAddress.Append($"/access-keys/{id}"));
        }

        public async Task<OutlineKey> TransferKeyToNewServer(Uri source, Uri dest, string id)
        {
            OutlineKey key = await CreateKeyAsync(dest);
            try
            {
                await DeleteKeyAsync(source, id);
            }
            catch (Exception)
            {
                await DeleteKeyAsync(dest, id);
                throw;
            }
            return key;
        }

        private async Task<Stream> SendRequestAsync(HttpMethod method, Uri uri, JsonNode? data = default)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient();

            using HttpRequestMessage httpRequest = new(method, uri);
            if (data is not null)
                httpRequest.Content = new StringContent(data.ToJsonString());

            HttpResponseMessage httpResponse = await httpClient.SendAsync(httpRequest);

            httpResponse.EnsureSuccessStatusCode();

            return await httpResponse.Content.ReadAsStreamAsync();
        }
    }
}
