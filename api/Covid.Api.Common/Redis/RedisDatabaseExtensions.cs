namespace Covid.Api.Common.Redis
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.FeatureManagement;
    using StackExchange.Redis;
    using StackExchange.Redis.Extensions.Core.Abstractions;

    public static class RedisDatabaseExtensions
    {
        /// <summary>
        /// Gets the item from the Cache or invokes given delegate to populate cache and return
        /// </summary>
        public static async Task<IQueryable<T>> GetOrCacheAside<T>(
            this IRedisDatabase cache,
            Func<IQueryable<T>> fetchDelegate,
            string hashKey,
            string hashPattern = "*",
            Func<T, string> keyBuilder = null,
            ILogger logger = null,
            IFeatureManager features = null)
            where T: ICacheable
        {
            if (features != null && !(await features.IsEnabledAsync(nameof(Features.Caching))))
            {
                logger?.LogInformation("Caching was disabled");
                return fetchDelegate.Invoke();
            }

            if (keyBuilder == null)
            {
                keyBuilder = x => x.ToCacheKeyString();
            }

            logger?.LogDebug("Retrieving from cache");
            var items = cache.HashScan<T>(hashKey, hashPattern).Select(x => x.Value).AsQueryable();
            if (!items.Any())
            {
                logger?.LogInformation("No Items were found in the cache, fetching and setting into cache");

                items = fetchDelegate.Invoke();
                await cache.HashSetAsync(hashKey, items.ToDictionary(keyBuilder, x => x));
            }

            logger?.LogDebug($"Retrieving {items.Count()} from cache");
            return items;
        }
    }
}