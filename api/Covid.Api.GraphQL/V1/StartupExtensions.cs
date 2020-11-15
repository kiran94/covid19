namespace Covid.Api.GraphQL.V1 
{
    using Covid.Api.GraphQL.V1.Query;
    using Covid.Api.GraphQL.V1.Schema;
    using global::GraphQL.Server;
    using global::GraphQL.Server.Ui.Playground;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class StartupExtensions
    {
        public static IServiceCollection AddGraphQLV1(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AppSchema>();
            services.AddScoped<AppQuery>();

            services.AddGraphQL(options =>
            {
                options.EnableMetrics = configuration.GetValue<bool>("GraphQL:EnableMetrics");
            })
            .AddErrorInfoProvider(options => 
            {
                var exposeException = configuration.GetValue<bool>("GraphQL:ExposeExceptions");
                options.ExposeCode = exposeException;
                options.ExposeData = exposeException;
                options.ExposeExceptionStackTrace = exposeException;
                options.ExposeExtensions = exposeException;
                options.ExposeCodes = exposeException;

                options.ExposeExceptionStackTrace = exposeException;
            })
            .AddSystemTextJson()
            .AddGraphTypes();

            return services;
        }

        public static IApplicationBuilder UseGraphQLV1(this IApplicationBuilder app)
        {
            const string endpoint = "/v1/graphql";
            const string playgroundEndpoint = "/v1/playground/";
            
            app.UseGraphQL<AppSchema>(path: endpoint);
            app.UseGraphQLPlayground(options: new GraphQLPlaygroundOptions() 
            {
                GraphQLEndPoint = endpoint,
                Path = playgroundEndpoint
            });
            return app;
        }
    }
}