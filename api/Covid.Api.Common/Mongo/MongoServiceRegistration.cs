namespace Covid.Api.Common.Mongo
{
    using System;
    using Covid.Api.Common.DataAccess;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;
    using MongoDB.Driver.Core.Events;

    public static class MongoServiceRegistration
    {
        public static IServiceCollection AddMongoService<TService, TImplementation>(
            this IServiceCollection services,
            string connectionString,
            string database)
            where TService: class
            where TImplementation : TService
            {
                services.AddSingleton<TService>(x => {

                    var settings = MongoClientSettings.FromConnectionString(connectionString);
                    settings.ClusterConfigurator = cb =>
                    {
                        cb.Subscribe<CommandStartedEvent>(
                            MongoInterceptors.CommandStartedEvent(x.GetRequiredService<ILogger<IMongoDatabase>>()));
                    };

                    var mongoDatabase = new MongoClient(settings).GetDatabase(database);

                    var impl = (TImplementation)Activator.CreateInstance(typeof(TImplementation), new MongoRepository(mongoDatabase));
                    return impl;
                });

                return services;
            }
    }
}