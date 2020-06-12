using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Covid.Api.GraphQL.Migrations
{
    public partial class AddedTimeSeriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "timeseries",
                schema: "public",
                columns: table => new
                {
                    country_region = table.Column<string>(type: "varchar", maxLength: 64, nullable: false),
                    province_state = table.Column<string>(type: "varchar", maxLength: 64, nullable: false),
                    county = table.Column<string>(type: "varchar", maxLength: 64, nullable: false),
                    field = table.Column<string>(type: "varchar", maxLength: 128, nullable: true),
                    date = table.Column<DateTime>(nullable: false),
                    value = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_timeseries", x => new { x.country_region, x.province_state, x.county });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "timeseries",
                schema: "public");
        }
    }
}
