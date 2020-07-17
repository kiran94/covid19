namespace Covid.Api.Grpc
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using System.Runtime.InteropServices;
    using Microsoft.AspNetCore.Server.Kestrel.Core;
    using Microsoft.Extensions.Configuration;
    using Serilog;
    using Serilog.Sinks.Elasticsearch;
    using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
    using Serilog.Exceptions;
    using Serilog.Exceptions.Core;
    using System.Reflection;

    public class Program
    {
        static string Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        public static void Main(string[] args)
        {
            Console.WriteLine("Configuring Logger");
            ConfigureLogger();

            try
            {
                Console.WriteLine("Application Starting");
                CreateHostBuilder(args).Build().Run();
            }
            catch(Exception ex)
            {
                Log.Fatal($"Failed to start {Assembly.GetExecutingAssembly().GetName().Name}", ex);
                Log.CloseAndFlush();
                throw;
            }
        }

        private static void ConfigureLogger()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{Environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails(
                    new DestructuringOptionsBuilder()
                    .WithDefaultDestructurers()
                    .WithDestructurers(new [] {new DbUpdateExceptionDestructurer()}))
                .Enrich.WithMachineName()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch(ConfigureElasticSink(configuration))
                .Enrich.WithProperty("Environment", Environment)
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration)
        {
            return new ElasticsearchSinkOptions(new Uri(configuration.GetValue<string>("ElasticSearch:Url")))
            {
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{Environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
            };
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        // Additional configuration is required to successfully run gRPC on macOS.
                        // For instructions on how to configure Kestrel and gRPC clients on macOS
                        // visit https://go.microsoft.com/fwlink/?linkid=2099682
                        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                        {
                            options.ListenLocalhost(6000, o => o.Protocols = HttpProtocols.Http2);
                        }
                    });
                    webBuilder.UseStartup<Startup>();
                }).ConfigureAppConfiguration(configuration =>
                {
                    configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    configuration.AddJsonFile($"appsettings.{Environment}.json", optional: true, reloadOnChange: true);
                    configuration.AddEnvironmentVariables();
                })
                .UseSerilog();
    }
}
