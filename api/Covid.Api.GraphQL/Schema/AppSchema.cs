namespace Covid.Api.GraphQL.Schema
{
    using System;
    using Covid.Api.GraphQL.Query;
    using global::GraphQL.Types;

    public class AppSchema : Schema
    {
        public AppSchema(IServiceProvider resolver) : base(resolver)
        {
            this.Query = resolver.GetService(typeof(AppQuery)) as ObjectGraphType;
        }
    }
}