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
    using System;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using OpenTracing.Util;
    using Serilog;
    using CorrelationId.DependencyInjection;
    using CorrelationId;
    using Covid.Api.Common.Services.Field;
    using Covid.Api.Common.Services.Countries;
    using Covid.Api.Common.Mongo;
    using Covid.Api.Common.Redis;
    using Microsoft.FeatureManagement;

    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        private static string ApplicationName = Assembly.GetEntryAssembly().GetName().Name;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // CORRELATION ID
            services.AddDefaultCorrelationId(options =>
            {
                options.AddToLoggingScope = true;
                options.IncludeInResponse = true;
                options.CorrelationIdGenerator = () => Guid.NewGuid().ToString();
            });

            // EF CORE
            services.AddDbContextPool<ApiContext>(builder =>
            {
                builder.UseNpgsql(this.Configuration.GetValue<string>("TimeseriesDatabase:ConnectionString"), options =>
                {
                    options.MigrationsAssembly(this.Configuration.GetValue<string>("EntityFramework:MigrationAssembly"));
                });

                builder.UseSnakeCaseNamingConvention();
                builder.EnableDetailedErrors(Environment.IsDevelopment());
                builder.EnableSensitiveDataLogging(Environment.IsDevelopment());
                builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<IRepository>(x => x.GetService<ApiContext>());

            // GRAPHQL
            services.AddScoped<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));
            services.AddScoped<AppSchema>();
            services.AddScoped<AppQuery>();
            services.AddGraphQL(options =>
            {
                options.ExposeExceptions = true;
            }).AddGraphTypes();

            // CORS
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });
            });

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

            // MONGO
            services.AddMongoService<IFieldService, FieldService>(
                this.Configuration.GetValue<string>("FieldsDatabase:ConnectionString"),
                this.Configuration.GetValue<string>("FieldsDatabase:DatabaseName"));

            services.AddMongoService<ICountryService, CountryService>(
                this.Configuration.GetValue<string>("CountryDatabase:ConnectionString"),
                this.Configuration.GetValue<string>("CountryDatabase:DatabaseName"));

            // REDIS
            services.AddRedisCache(this.Configuration.GetSection("Redis"));

            services.AddFeatureManagement();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCorrelationId();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors();

            app.UseGraphQL<AppSchema>();
            app.UseGraphQLPlayground(options: new GraphQLPlaygroundOptions());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
