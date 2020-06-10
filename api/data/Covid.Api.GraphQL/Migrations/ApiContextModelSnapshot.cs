﻿// <auto-generated />
using System;
using Covid.Api.Common.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Covid.Api.GraphQL.Migrations
{
    [DbContext(typeof(ApiContext))]
    partial class ApiContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Covid.Api.Common.Entities.Country", b =>
                {
                    b.Property<string>("CountryRegion")
                        .HasColumnName("country_region")
                        .HasColumnType("varchar")
                        .HasMaxLength(64);

                    b.Property<string>("ProvinceState")
                        .HasColumnName("province_state")
                        .HasColumnType("varchar")
                        .HasMaxLength(64);

                    b.Property<string>("County")
                        .HasColumnName("county")
                        .HasColumnType("varchar")
                        .HasMaxLength(64);

                    b.Property<string>("Iso2")
                        .HasColumnName("iso2")
                        .HasColumnType("char(2)");

                    b.Property<string>("Iso3")
                        .HasColumnName("iso3")
                        .HasColumnType("char(3)");

                    b.Property<double?>("Latitude")
                        .HasColumnName("latitude")
                        .HasColumnType("double precision");

                    b.Property<double?>("Longitude")
                        .HasColumnName("longitude")
                        .HasColumnType("double precision");

                    b.Property<double?>("Population")
                        .HasColumnName("population")
                        .HasColumnType("double precision");

                    b.HasKey("CountryRegion", "ProvinceState", "County")
                        .HasName("pk_country");

                    b.ToTable("country","public");
                });

            modelBuilder.Entity("Covid.Api.Common.Entities.TimeSeries", b =>
                {
                    b.Property<string>("CountryRegion")
                        .HasColumnName("country_region")
                        .HasColumnType("varchar")
                        .HasMaxLength(64);

                    b.Property<string>("ProvinceState")
                        .HasColumnName("province_state")
                        .HasColumnType("varchar")
                        .HasMaxLength(64);

                    b.Property<string>("County")
                        .HasColumnName("county")
                        .HasColumnType("varchar")
                        .HasMaxLength(64);

                    b.Property<DateTime>("Date")
                        .HasColumnName("date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Field")
                        .HasColumnName("field")
                        .HasColumnType("varchar")
                        .HasMaxLength(128);

                    b.Property<double?>("Value")
                        .HasColumnName("value")
                        .HasColumnType("double precision");

                    b.HasKey("CountryRegion", "ProvinceState", "County", "Date", "Field")
                        .HasName("pk_timeseries");

                    b.ToTable("timeseries","public");
                });
#pragma warning restore 612, 618
        }
    }
}
