namespace Covid.Api.GraphQL.V1.Schema
{
    using System;
    using Covid.Api.GraphQL.V1.Query;
    using global::GraphQL.Types;

    public class AppSchema : Schema
    {
        public AppSchema(IServiceProvider resolver) : base(resolver)
        {
            this.Query = resolver.GetService(typeof(AppQuery)) as ObjectGraphType;
        }
    }
}