namespace Covid.Api.Grpc
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    // using OpenTracing;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    // using OpenTracing.Util;
    // using Serilog;
    // using CorrelationId;
    using Covid.Api.Common.Services.Field;
    using Covid.Api.Common.Services.Countries;
    using Covid.Api.Common.Mongo;
    using Covid.Api.Common.Redis;
    using Microsoft.FeatureManagement;
    using Covid.Api.Common.Services.TimeSeries;
    using Covid.Api.Common.Configuration;
    using Covid.Api.Grpc.Services;
    using Serilog;

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
            services.AddScoped<ITimeSeriesService, TimeSeriesService>();

            services.AddGrpc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSerilogRequestLogging();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<CovidDataService>();
                // endpoints.MapGet("/", async context =>
                // {
                //     await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                // });
            });
        }
    }
}
