namespace Covid.Api.GraphQL.Schema
{
    using Covid.Api.GraphQL.Query;
    using global::GraphQL;
    using global::GraphQL.Types;

    public class AppSchema : Schema
    {
        public AppSchema(IDependencyResolver resolver) : base(resolver)
        {
            this.Query = resolver.Resolve<AppQuery>();
        }
    }
}