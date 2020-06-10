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

    public class AppQuery : ObjectGraphType
    {
        public AppQuery(ApiContext sql, ILogger<AppQuery> logger)
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
                    logger.LogInformation("Getting Country Information");

                    var countries = sql.Set<Country>()
                        .OrderBy(x => x.CountryRegion)
                        .ThenBy(x => x.ProvinceState)
                        .AsQueryable();

                    if (context.TryGetArgument<string>("query", out var query))
                    {
                        query = "%" + query + "%";
                        countries = countries.Where(
                            x => EF.Functions.Like(x.CountryRegion, query)
                                || EF.Functions.Like(x.ProvinceState, query)
                                || EF.Functions.Like(x.County, query));
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
                    new QueryArgument<StringGraphType>() { Name = "country_region", Description = "The Country/Region to Filter on" },
                    new QueryArgument<StringGraphType>() { Name = "province_state", Description = "The Province/State to Filter on", DefaultValue = "" },
                    new QueryArgument<StringGraphType>() { Name = "county", Description = "The County to Filter on", DefaultValue = "" },
                    new QueryArgument<StringGraphType>() { Name = "field", Description = "The Field to Filter on" },
                    new QueryArgument<DateTimeGraphType>() { Name = "date", Description = "The Time to Filter on" },
                    new QueryArgument<IntGraphType>() { Name = "take", Description = "(Pagination) only take specified number of results" },
                    new QueryArgument<IntGraphType>() { Name = "skip", Description = "(Pagination) skip the specified number of results" }
                },
                resolve: async context =>
                {
                    logger.LogInformation("Getting TimeSeries Information");

                    var timeseries = sql.Set<TimeSeries>().OrderByDescending(x => x.Date).AsQueryable();

                    if (context.TryGetArgument<string>("country_region", out var country))
                    {
                        timeseries = timeseries.Where(x => x.CountryRegion == country);
                    }

                    if (context.TryGetArgument<string>("province_state", out var province))
                    {
                        timeseries = timeseries.Where(x => x.ProvinceState == province);
                    }

                    if (context.TryGetArgument<string>("county", out var county))
                    {
                        timeseries = timeseries.Where(x => x.County == county);
                    }

                    if (context.TryGetArgument<string>("field", out var field))
                    {
                        timeseries = timeseries.Where(x => x.Field == field);
                    }

                    if (context.TryGetArgument<DateTime>("date", out var date))
                    {
                        timeseries = timeseries.Where(x => x.Date == date);
                    }

                    if (context.TryGetArgument<int>("take", out var take)) timeseries = timeseries.Take(take);
                    if (context.TryGetArgument<int>("skip", out var skip)) timeseries = timeseries.Skip(skip);

                    return await timeseries.ToListAsync(context.CancellationToken);
                });
            #endregion
        }
    }
}