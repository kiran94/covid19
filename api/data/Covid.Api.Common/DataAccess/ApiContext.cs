namespace Covid.Api.Common.DataAccess
{
    using Covid.Api.Common.Entities;
    using Microsoft.EntityFrameworkCore;

    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> context): base(context)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>(options =>
            {
                options.ToTable("country", "public");
                options.HasKey(x => new { x.CountryRegion, x.ProvinceState, x.County });

                options.Property(p => p.CountryRegion).HasColumnType("varchar").HasMaxLength(64);
                options.Property(p => p.ProvinceState).HasColumnType("varchar").HasMaxLength(64);
                options.Property(p => p.County).HasColumnType("varchar").HasMaxLength(64);

                options.Property(p => p.Latitude).HasColumnType("double precision");
                options.Property(p => p.Longitude).HasColumnType("double precision");
                options.Property(p => p.Population).HasColumnType("double precision");

                options.Property(p => p.Iso2).HasColumnType("char(2)");
                options.Property(p => p.Iso3).HasColumnType("char(3)");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}