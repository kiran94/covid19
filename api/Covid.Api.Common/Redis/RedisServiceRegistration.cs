namespace Covid.Api.Common.Redis
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using StackExchange.Redis.Extensions.Core;
    using StackExchange.Redis.Extensions.Core.Abstractions;
    using StackExchange.Redis.Extensions.Core.Configuration;
    using StackExchange.Redis.Extensions.Core.Implementations;
    using StackExchange.Redis.Extensions.System.Text.Json;
    using StackExchange.Redis.Extensions.MsgPack;

    /// <summary>
    /// Responsible for registering Redis based services.
    /// </summary>
    public static class RedisServiceRegistration
    {
        /// <summary>
        /// Registers Connections to the Redis Database to the DI system.
        /// </summary>
        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRedisCacheClient, RedisCacheClient>();
            services.AddSingleton<IRedisCacheConnectionPoolManager, RedisCacheConnectionPoolManager>();
            services.AddSingleton<ISerializer, MsgPackObjectSerializer>();
            services.AddSingleton(configuration.Get<RedisConfiguration>());
            return services;
        }

        /// <summary>
        /// Obtain an interactive connection to a database inside redis
        /// </summary>
        public static IRedisDatabase GetDatabase(this IRedisCacheClient @this, RedisDatabase database) => @this.GetDb((int)database);
    }

    /// <summary>
    /// Represents a Database on the Redis Server.
    /// </summary>
    public enum RedisDatabase
    {
        Default = -1,
        Country = 0
    }

    public interface ICacheable
    {
        string ToCacheKeyString();
    }

    public enum CacheHashKey
    {
        Default = 0,
        AllCountries = 1
    }
}