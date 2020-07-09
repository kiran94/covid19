namespace Covid.Api.Common.DataAccess
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using Covid.Api.Common.DataAccess.Attribute;
    using MongoDB.Driver;

    public class MongoRepository : IRepository
    {
        private readonly IMongoDatabase database;
        private static ConcurrentDictionary<Type, string> typeToCollectionCache = new ConcurrentDictionary<Type, string>();

        public MongoRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public IQueryable<T> Query<T>() where T : class
        {
            var targetType = typeof(T);
            if (!typeToCollectionCache.ContainsKey(targetType))
            {
                var nameAttribute = (MongoCollectionAttribute) System.Attribute.GetCustomAttribute(targetType, typeof(MongoCollectionAttribute));
                typeToCollectionCache.TryAdd(targetType, nameAttribute.Name);
            }

            var collectionName = typeToCollectionCache[targetType];

            return this.database.GetCollection<T>(collectionName).AsQueryable();
        }
    }
}