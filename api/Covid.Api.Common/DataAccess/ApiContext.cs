namespace Covid.Api.Common.DataAccess
{
    using System.Linq;
    using Covid.Api.Common.Services.TimeSeries;
    using Microsoft.EntityFrameworkCore;

    public class ApiContext : DbContext, IRepository
    {
        public ApiContext(DbContextOptions<ApiContext> context): base(context)
        {
        }

        public IQueryable<T> Query<T>() where T : class => this.Set<T>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeSeries>(options =>
            {
                options.ToTable("timeseries", "public");
                options.HasKey(x => new { x.CountryRegion, x.ProvinceState, x.County, x.Date, x.Field });

                options.Property(p => p.CountryRegion).HasColumnType("varchar").HasMaxLength(64);
                options.Property(p => p.ProvinceState).HasColumnType("varchar").HasMaxLength(64);
                options.Property(p => p.County).HasColumnType("varchar").HasMaxLength(64);

                options.Property(p => p.Field).HasColumnType("varchar").HasMaxLength(128);
                options.Property(p => p.Value).HasColumnType("double precision");
                options.Property(p => p.Date);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
