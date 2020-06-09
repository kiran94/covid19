namespace Covid.Api.GraphQL.Query
{
    using Covid.Api.Common.DataAccess;
    using Covid.Api.Common.Entities;
    using Covid.Api.GraphQL.Types;
    using global::GraphQL.Types;
    using Microsoft.EntityFrameworkCore;

    public class AppQuery : ObjectGraphType
    {
        public AppQuery(ApiContext sql)
        {
            this.FieldAsync<ListGraphType<CountryType>>(
                name: "countries",
                description: "Get Country Information",
                arguments: new QueryArguments()
                {
                    new QueryArgument<StringGraphType>() { Name = "Hello" }
                },
                resolve: async context =>
                {
                    return await sql.Set<Country>().ToListAsync(context.CancellationToken);
                });
        }
    }
}