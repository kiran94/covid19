namespace Covid.Api.GraphQL
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using OpenTracing;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using OpenTracing.Util;
    using Serilog;
    using CorrelationId;
    using Covid.Api.Common.Services.Field;
    using Covid.Api.Common.Services.Countries;
    using Covid.Api.Common.Mongo;
    using Covid.Api.Common.Redis;
    using Microsoft.FeatureManagement;
    using Covid.Api.Common.Services.TimeSeries;
    using Covid.Api.Common.Configuration;
    using Covid.Api.GraphQL.V1;

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
            services.AddFeatureManagement();
            services.AddCommonCorrelation();
            services.AddCommonCors();
            services.AddCommonPostgresDatabase(this.Configuration);
            services.AddRedisCache(this.Configuration.GetSection("Redis"));

            services.AddScoped<ITimeSeriesService, TimeSeriesService>();

            // GRAPHQL
            services.AddGraphQLV1(this.Configuration);

            // OPENTRACING
            services.AddSingleton<ITracer>(provider =>
            {
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

            app.UseGraphQLV1();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
