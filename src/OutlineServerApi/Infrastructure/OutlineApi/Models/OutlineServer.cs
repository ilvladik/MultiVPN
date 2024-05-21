namespace OutlineServerApi.Infrastructure.OutlineApi.Models
{
    public class OutlineServer
    {
        public string Name { get; set; } = string.Empty;
        public string ServerId { get; set; } = string.Empty;
        public bool MetricEnabled { get; set; }
        public string Version { get; set; } = string.Empty;
        public int PortForNewAccessKeys { get; set; }
        public string HostnameForAccessKeys { get; set; } = string.Empty;
    };
}
