using System.Threading;
using System;
namespace Covid.Api.GraphQL.Query
{
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

    public class AppQuery : ObjectGraphType
    {
        public AppQuery(ApiContext sql, ILogger<AppQuery> logger, ITracer tracer)
        {
            #region countries
            this.FieldAsync<ListGraphType<CountryType>>(
                name: "countries",
                description: "Get Country Information",
                arguments: new QueryArguments()
                {
                    new QueryArgument<StringGraphType>() { Name = "query", Description = "Fuzzy Search Country/Region/Province/State/County" },
                    new QueryArgument<IntGraphType>() { Name = "take", Description = "(Pagination) only take specified number of results" },
                    new QueryArgument<IntGraphType>() { Name = "skip", Description = "(Pagination) skip the specified number of results" }
                },
                resolve: async context =>
                {
                    tracer.ActiveSpan.SetOperationName("GRAPHQL " + string.Join(".", context.Path)).WithGraphQLTags(context);

                    logger.LogInformation("Getting Country Information");

                    var countries = sql.Set<Country>()
                        .OrderBy(x => x.CountryRegion)
                        .ThenBy(x => x.ProvinceState)
                        .AsQueryable();

                    if (context.TryGetArgument<string>("query", out var query))
                    {
                        query = "%" + query + "%";
                        countries = countries.Where(
                            x => EF.Functions.ILike(x.CountryRegion, query)
                                || EF.Functions.ILike(x.ProvinceState, query)
                                || EF.Functions.ILike(x.County, query));
                    }

                    if (context.TryGetArgument<int>("take", out var take)) countries = countries.Take(take);
                    if (context.TryGetArgument<int>("skip", out var skip)) countries = countries.Skip(skip);

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
                    new QueryArgument<ListGraphType<StringGraphType>>() { Name = "country_region", Description = "The Country/Region(s) to Filter on"},
                    new QueryArgument<ListGraphType<StringGraphType>>() { Name = "province_state", Description = "The Province/State(s) to Filter on"},
                    new QueryArgument<ListGraphType<StringGraphType>>() { Name = "counties", Description = "The Counties to Filter on" },
                    new QueryArgument<ListGraphType<StringGraphType>>() { Name = "fields", Description = "The Field to Filter on" },
                    new QueryArgument<ListGraphType<DateTimeGraphType>>() { Name = "dates", Description = "The Time to Filter on" },
                    new QueryArgument<IntGraphType>() { Name = "take", Description = "(Pagination) only take specified number of results" },
                    new QueryArgument<IntGraphType>() { Name = "skip", Description = "(Pagination) skip the specified number of results" }
                },
                resolve: async context =>
                {
                    tracer.ActiveSpan.SetOperationName("GRAPHQL " + string.Join(".", context.Path)).WithGraphQLTags(context);

                    logger.LogInformation("Getting TimeSeries Information");
                    using var _ = logger.BeginScope(context.Arguments);

                    var timeseries = sql.Set<TimeSeries>().OrderByDescending(x => x.Date).AsQueryable();

                    if (context.TryGetArgument<List<string>>("country_region", out var countries))
                    {
                        timeseries = timeseries.Where(x => countries.Contains(x.CountryRegion));
                    }

                    if (context.TryGetArgument<List<string>>("province_state", out var provinces))
                    {
                        timeseries = timeseries.Where(x => provinces.Contains(x.ProvinceState));
                    }

                    if (context.TryGetArgument<List<string>>("counties", out var counties))
                    {
                        timeseries = timeseries.Where(x => counties.Contains(x.County));
                    }

                    if (context.TryGetArgument<List<string>>("fields", out var fields))
                    {
                        timeseries = timeseries.Where(x => fields.Contains(x.Field));
                    }

                    if (context.TryGetArgument<List<DateTime>>("dates", out var dates))
                    {
                        timeseries = timeseries.Where(x => dates.Contains(x.Date));
                    }

                    if (context.TryGetArgument<int>("skip", out var skip)) timeseries = timeseries.Skip(skip);
                    if (context.TryGetArgument<int>("take", out var take)) timeseries = timeseries.Take(take);

                    return await timeseries.ToListAsync(context.CancellationToken);
                });
            #endregion
        }
    }
}