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
                    new QueryArgument<StringGraphType>() { Name = "query" }
                },
                resolve: async context =>
                {
                    logger.LogInformation("Getting Country Information");

                    var countries = sql.Set<Country>().AsQueryable();

                    if (context.TryGetArgument<string>("query", out var query))
                    {
                        query = "%" + query + "%";
                        countries = countries.Where(
                            x => EF.Functions.Like(x.CountryRegion, query)
                                || EF.Functions.Like(x.ProvinceState, query)
                                || EF.Functions.Like(x.County, query));
                    }

                    return await countries.ToListAsync(context.CancellationToken);
                });
        }
    }
}