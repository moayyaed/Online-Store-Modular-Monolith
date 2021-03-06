using Common.Caching.Caching;
using Common.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Common.Caching
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            var redisConnectionString = configuration.GetConnectionString("RedisConnectionString");

            services.AddOptions<CachingOptions>().Bind(configuration.GetSection("Caching")).ValidateDataAnnotations();


            if (!string.IsNullOrEmpty(redisConnectionString))
            {
                services.AddOptions<RedisCachingOptions>().Bind(configuration.GetSection("Caching:Redis")).ValidateDataAnnotations();

                var redis = ConnectionMultiplexer.Connect(redisConnectionString);
                services.AddSingleton(redis.GetSubscriber());
                services.AddSingleton<IExtendedMemoryCache, RedisMemoryCache>();
            }
            else
            {
                //Use MemoryCache decorator to use global platform cache settings
                services.AddSingleton<IExtendedMemoryCache, ExtendedMemoryCache>();
            }

            return services;
        }
    }
}
