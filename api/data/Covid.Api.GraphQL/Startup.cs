namespace Covid.Api.GraphQL
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Covid.Api.Common.DataAccess;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
