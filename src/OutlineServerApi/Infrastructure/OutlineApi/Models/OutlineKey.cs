namespace OutlineServerApi.Infrastructure.OutlineApi.Models
{
    public class OutlineKey
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Method { get; set; } = string.Empty;
        public DataLimit? DataLimit { get; set; }
        public string AccessUrl { get; set; } = string.Empty;
    };
}
