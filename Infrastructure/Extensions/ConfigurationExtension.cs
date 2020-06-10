using Microsoft.Extensions.Configuration;

namespace Hal.Infrastructure.Extensions
{
    public static class ConfigurationExtension
    {
        public static TConfig BindConfig<TConfig>(this IConfiguration configuration)
            where TConfig : class, new()
        {
            var config = new TConfig();
            configuration.Bind(typeof(TConfig).Name, config);
            return config;
        }
    }
}
