namespace Covid.Api.Common.Configuration
{
    using System;
    using CorrelationId.DependencyInjection;
    using Covid.Api.Common.DataAccess;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCommonCorrelation(this IServiceCollection services)
        {
            services.AddDefaultCorrelationId(options =>
            {
                options.AddToLoggingScope = true;
                options.IncludeInResponse = true;
                options.CorrelationIdGenerator = () => Guid.NewGuid().ToString();
            });

            return services;
        }

        public static IServiceCollection AddCommonCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });
            });

            return services;
        }

        public static IServiceCollection AddCommonPostgresDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<ApiContext>(builder =>
            {
                builder.UseNpgsql(configuration.GetValue<string>("TimeseriesDatabase:ConnectionString"), options =>
                {
                    options.MigrationsAssembly(configuration.GetValue<string>("EntityFramework:MigrationAssembly"));
                });

                builder.UseSnakeCaseNamingConvention();
                builder.EnableDetailedErrors(configuration.GetValue<bool>("EntityFramework:EnableDetailedErrors"));
                builder.EnableSensitiveDataLogging(configuration.GetValue<bool>("EntityFramework:EnableSensitiveDataLogging"));
                builder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });

            services.AddScoped<IRepository>(x => x.GetService<ApiContext>());
            return services;
        }
    }
}