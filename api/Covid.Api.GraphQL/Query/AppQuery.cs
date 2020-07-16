namespace Covid.Api.GraphQL.Query
{
    using System;
    using System.Linq;
    using Covid.Api.Common.DataAccess;
    using Covid.Api.Common.Entities;
    using Covid.Api.GraphQL.Extensions;
    using Covid.Api.GraphQL.Types;
    using global::GraphQL.Types;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using OpenTracing;
    using Covid.Api.Common.Services.Field;
    using Covid.Api.Common.Services.Countries;
    using StackExchange.Redis.Extensions.Core.Abstractions;
    using Covid.Api.Common.Redis;

    public class AppQuery : ObjectGraphType
    {
        public AppQuery(
            IEnumerable<IRepository> repositories,
            ILogger<AppQuery> logger,
            ITracer tracer,
            IFieldService fields,
            ICountryService countriesService,
            IRedisCacheClient redis)
        {
            var dataRepository = repositories.First(x => x is ApiContext);

            #region countries
            this.FieldAsync<ListGraphType<CountryType>>(
                name: "countries",
                description: "Get Country Information",
                arguments: new QueryArguments()
                {
                    Parameters.Argument<StringGraphType>(Parameters.Queries),
                    Parameters.Argument<IntGraphType>(Parameters.Take),
                    Parameters.Argument<IntGraphType>(Parameters.Skip)
                },
                resolve: async context =>
                {
                    tracer.ActiveSpan.SetOperationName("GRAPHQL " + string.Join(".", context.Path)).WithGraphQLTags(context);

                    logger.LogDebug("Checking Cache for Country Information");
                    var cache = redis.GetDatabase(RedisDatabase.Country);

                    var countries = await cache.GetOrCacheAside(
                        () => countriesService.Query().ToList().AsQueryable(),
                        nameof(CacheHashKey.AllCountries).ToLower(),
                        logger: logger);

                    if (context.TryGetArgument<string>(Parameters.Queries, out var query))
                    {
                        countries = countries.Where(x => x.CountryRegion == query);
                    }

                    if (context.TryGetArgument<int>(Parameters.Skip, out var skip)) countries = countries.Skip(skip);
                    if (context.TryGetArgument<int>(Parameters.Take, out var take)) countries = countries.Take(take);

                    countries = countries.OrderBy(x => x.CountryRegion).ThenBy(x => x.ProvinceState);

                    if (countries is IAsyncEnumerable<Country>)
                    {
                        return await countries.ToListAsync(context.CancellationToken);
                    }
                    else
                    {
                        return countries.ToList();
                    }
                });
            #endregion

            #region timeseries
            this.FieldAsync<ListGraphType<TimeSeriesType>>(
                name: "timeseries",
                description: "Gets the COVID-19 Timeseries Data",
                arguments: new QueryArguments() {
                    Parameters.Argument<ListGraphType<StringGraphType>>(Parameters.CountryRegion),
                    Parameters.Argument<ListGraphType<StringGraphType>>(Parameters.ProvinceState),
                    Parameters.Argument<ListGraphType<StringGraphType>>(Parameters.Counties),
                    Parameters.Argument<ListGraphType<StringGraphType>>(Parameters.Fields),
                    Parameters.Argument<ListGraphType<DateTimeGraphType>>(Parameters.Dates),
                    Parameters.Argument<IntGraphType>(Parameters.Take),
                    Parameters.Argument<IntGraphType>(Parameters.Skip),
                    Parameters.Argument<BooleanGraphType>(Parameters.Chronological),
                    Parameters.Argument<BooleanGraphType>(Parameters.OrderByValueAscending),
                    Parameters.Argument<BooleanGraphType>(Parameters.OrderByValueDescending)
                },
                resolve: async context =>
                {
                    tracer.ActiveSpan.SetOperationName("GRAPHQL " + string.Join(".", context.Path)).WithGraphQLTags(context);

                    logger.LogInformation("Getting TimeSeries Information");
                    using var _ = logger.BeginScope(context.Arguments);

                    var timeseries = dataRepository.Query<TimeSeries>();

                    if (context.TryGetArgument<bool>(Parameters.Chronological, out var chronological) && chronological)
                    {
                        timeseries = timeseries.OrderBy(x => x.Date);
                    }
                    else if (context.TryGetArgument<bool>(Parameters.OrderByValueAscending, out var orderByValueAsc) && orderByValueAsc)
                    {
                        timeseries = timeseries.OrderBy(x => x.Value);
                    }
                    else if (context.TryGetArgument<bool>(Parameters.OrderByValueDescending, out var orderByValueDesc) && orderByValueDesc)
                    {
                        timeseries = timeseries.OrderByDescending(x => x.Value);
                    }
                    else
                    {
                        timeseries = timeseries.OrderByDescending(x => x.Date);
                    }

                    if (context.TryGetArgument<List<string>>(Parameters.CountryRegion, out var countries))
                    {
                        timeseries = timeseries.Where(x => countries.Contains(x.CountryRegion));
                    }

                    if (context.TryGetArgument<List<string>>(Parameters.ProvinceState, out var provinces))
                    {
                        timeseries = timeseries.Where(x => provinces.Contains(x.ProvinceState));
                    }

                    if (context.TryGetArgument<List<string>>(Parameters.Counties, out var counties))
                    {
                        timeseries = timeseries.Where(x => counties.Contains(x.County));
                    }

                    if (context.TryGetArgument<List<string>>(Parameters.Fields, out var fields))
                    {
                        timeseries = timeseries.Where(x => fields.Contains(x.Field));
                    }

                    if (context.TryGetArgument<List<DateTime>>(Parameters.Dates, out var dates))
                    {
                        timeseries = timeseries.Where(x => dates.Contains(x.Date));
                    }

                    if (context.TryGetArgument<int>(Parameters.Skip, out var skip)) timeseries = timeseries.Skip(skip);
                    if (context.TryGetArgument<int>(Parameters.Take, out var take)) timeseries = timeseries.Take(take);

                    return await timeseries.ToListAsync(context.CancellationToken);
                });
            #endregion

            #region fields
            this.FieldAsync<ListGraphType<GraphQL.Types.FieldType>>(
                "fields",
                description: "Gets Fields tracked under data",
                resolve: async context => {
                    tracer.ActiveSpan.SetOperationName("GRAPHQL " + string.Join(".", context.Path)).WithGraphQLTags(context);

                    var retrievedFields = fields.Query();

                    if (retrievedFields is IAsyncEnumerable<Country>)
                    {
                        return await retrievedFields.ToListAsync(context.CancellationToken);
                    }
                    else
                    {
                        return retrievedFields.ToList();
                    }
                });
            #endregion
        }
    }
}