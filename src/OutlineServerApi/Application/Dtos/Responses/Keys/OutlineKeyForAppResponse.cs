using System.Text.Json.Serialization;

namespace OutlineServerApi.Application.Dtos.Responses.Keys
{
    public sealed class OutlineKeyForAppResponse()
    {
        [JsonPropertyName("server")]
        public required string Server { get; init; }

        [JsonPropertyName("server_port")]
        public required string Port { get; init; }

        [JsonPropertyName("password")]
        public required string Password { get; init; }

        [JsonPropertyName("method")]
        public required string Method { get; init; }
    }
}
