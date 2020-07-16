namespace Covid.Api.Common.Redis
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
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
            ILogger logger = null)
            where T: ICacheable
        {
            if (keyBuilder == null)
            {
                keyBuilder = x => x.ToCacheKeyString();
            }

            var items = cache.HashScan<T>(hashKey, hashPattern).Select(x => x.Value).AsQueryable();
            if (!items.Any())
            {
                logger?.LogInformation("No Items were found in the cache, fetching and setting into cache");

                items = fetchDelegate.Invoke();
                await cache.HashSetAsync(hashKey, items.ToDictionary(keyBuilder, x => x));
            }

            return items;
        }
    }
}