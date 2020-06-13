
namespace Covid.Api.GraphQL.Query
{
    using System.Threading;
    using System;
    using System.Linq;
    using Covid.Api.Common.DataAccess;
    using Covid.Api.Common.Entities;
    using Covid.Api.GraphQL.Extensions;
    using Covid.Api.GraphQL.Types;
    using global::GraphQL.Types;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using OpenTracing;
    using Covid.Api.GraphQL.Services;

    public class AppQuery : ObjectGraphType
    {
        public AppQuery(ApiContext sql, ILogger<AppQuery> logger, ITracer tracer, ITimeSeriesService timeSeriesService)
        {
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

                    logger.LogInformation("Getting Country Information");

                    var countries = sql.Set<Country>()
                        .OrderBy(x => x.CountryRegion)
                        .ThenBy(x => x.ProvinceState)
                        .AsQueryable();

                    if (context.TryGetArgument<object, string>(Parameters.Queries, out var query))
                    {
                        query = "%" + query + "%";
                        countries = countries.Where(
                            x => EF.Functions.ILike(x.CountryRegion, query)
                                || EF.Functions.ILike(x.ProvinceState, query)
                                || EF.Functions.ILike(x.County, query));
                    }

                    if (context.TryGetArgument<object, int>(Parameters.Take, out var take)) countries = countries.Take(take);
                    if (context.TryGetArgument<object, int>(Parameters.Skip, out var skip)) countries = countries.Skip(skip);

                    return await countries
                        .OrderBy(x => x.CountryRegion)
                        .ToListAsync(context.CancellationToken);
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
                    Parameters.Argument<IntGraphType>(Parameters.Skip)
                },
                resolve: async context =>
                {
                    tracer.ActiveSpan.SetOperationName("GRAPHQL " + string.Join(".", context.Path)).WithGraphQLTags(context);

                    logger.LogInformation("Getting TimeSeries Information");
                    using var _ = logger.BeginScope(context.Arguments);

                    var timeseries = sql.Set<TimeSeries>().OrderByDescending(x => x.Date).AsQueryable();
                    var result = await timeSeriesService.Query(timeseries, context);

                    return result.ToListAsync(context.CancellationToken);
                });
            #endregion
        }
    }
}