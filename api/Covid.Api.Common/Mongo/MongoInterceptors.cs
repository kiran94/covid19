namespace Covid.Api.Common.Mongo
{
    using System;
    using Microsoft.Extensions.Logging;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using MongoDB.Driver.Core.Events;

    /// <summary>
    /// Common Interceptors for Mongo Queries.
    /// </summary>
    public static class MongoInterceptors
    {
        /// <summary>
        /// Intercepts the start of a Mongo Query.
        /// </summary>
        public static Action<CommandStartedEvent> CommandStartedEvent(
            ILogger<IMongoDatabase> logger)
        {
            return e =>
            {
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation(
                        "Mongo Query {0} Running {1}",
                        e.CommandName,
                        e.Command.ToJson());
                }
            };
        }
    }
}