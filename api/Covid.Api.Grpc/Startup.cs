namespace Covid.Api.Grpc
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
    using Microsoft.FeatureManagement;
    using Covid.Api.Common.Services.TimeSeries;
    using Covid.Api.Common.Configuration;
    using Covid.Api.Grpc.Services;
    using Serilog;
    using System;

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
            services.AddGrpc();
            services.AddGrpcReflection();
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
                endpoints.MapGrpcReflectionService();
            });
        }
    }
}
