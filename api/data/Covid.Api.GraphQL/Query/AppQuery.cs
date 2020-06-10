using System.Collections.Concurrent;
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

    public class AppQuery : ObjectGraphType
    {
        public AppQuery(ApiContext sql, ILogger<AppQuery> logger)
        {
            this.FieldAsync<ListGraphType<CountryType>>(
                name: "countries",
                description: "Get Country Information",
                arguments: new QueryArguments()
                {
                    new QueryArgument<StringGraphType>() { Name = "query", Description = " Search Country/Region/Province/State/County" },
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
        }
    }
}