namespace Covid.Api.GraphQL
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Covid.Api.Common.DataAccess;
    using global::GraphQL;
    using Covid.Api.GraphQL.Schema;
    using global::GraphQL.Server;
    using global::GraphQL.Server.Ui.Playground;
    using Covid.Api.GraphQL.Query;
    using OpenTracing;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using OpenTracing.Util;

    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // EF CORE
            services.AddDbContextPool<ApiContext>(builder =>
            {
                var host = System.Environment.GetEnvironmentVariable("POSTGRES_HOST");
                var port = System.Environment.GetEnvironmentVariable("POSTGRES_PORT");
                var user = System.Environment.GetEnvironmentVariable("POSTGRES_USER");
                var password = System.Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
                var database = System.Environment.GetEnvironmentVariable("COVID_DATABASE_NAME");

                builder.UseNpgsql($"Host={host};Database={database};Username={user};Password={password}", options =>
                {
                    options.MigrationsAssembly("Covid.Api.GraphQL");
                });

                builder.UseSnakeCaseNamingConvention();
                builder.EnableDetailedErrors(Environment.IsDevelopment());
                builder.EnableSensitiveDataLogging(Environment.IsDevelopment());
            });

            // GRAPHQL
            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.AddScoped<AppSchema>();
            services.AddScoped<AppQuery>();
            services.AddGraphQL(options =>
            {
                options.ExposeExceptions = true;
            }).AddGraphTypes();

            // OPENTRACING
            services.AddSingleton<ITracer>(provider =>
            {
                var serviceName = Assembly.GetEntryAssembly().GetName().Name;
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

                var config = Jaeger.Configuration.FromIConfiguration(
                    loggerFactory,
                    this.Configuration.GetSection("Jaeger"));

                var tracer = config.GetTracer();
                GlobalTracer.Register(tracer);

                return tracer;
            });

            services.AddOpenTracing();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseGraphQL<AppSchema>();
            app.UseGraphQLPlayground(options: new GraphQLPlaygroundOptions());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
