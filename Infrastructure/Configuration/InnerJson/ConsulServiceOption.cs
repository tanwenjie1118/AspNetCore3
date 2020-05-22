namespace Infrastructure.Configuration
{
    public class ConsulServiceOption
    {
        public string ServerHost { get; set; }
        public string ClientHost { get; set; }
        public string ClientName { get; set; }
        public string ClientIp { get; set; }
        public int ClientPort { get; set; }
        public string HealthCheckHttp { get; set; }
    }
}
