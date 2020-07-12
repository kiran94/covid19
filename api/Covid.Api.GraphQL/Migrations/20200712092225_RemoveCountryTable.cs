﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace Covid.Api.GraphQL.Migrations
{
    public partial class RemoveCountryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "country",
                schema: "public");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "country",
                schema: "public",
                columns: table => new
                {
                    country_region = table.Column<string>(type: "varchar", maxLength: 64, nullable: false),
                    province_state = table.Column<string>(type: "varchar", maxLength: 64, nullable: false),
                    county = table.Column<string>(type: "varchar", maxLength: 64, nullable: false),
                    iso2 = table.Column<string>(type: "char(2)", nullable: true),
                    iso3 = table.Column<string>(type: "char(3)", nullable: true),
                    latitude = table.Column<double>(type: "double precision", nullable: true),
                    longitude = table.Column<double>(type: "double precision", nullable: true),
                    population = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_country", x => new { x.country_region, x.province_state, x.county });
                });
        }
    }
}
