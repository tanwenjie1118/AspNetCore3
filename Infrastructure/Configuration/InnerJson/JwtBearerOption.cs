namespace Hal.Infrastructure.Configuration
{
    public class JwtBearerOption
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
