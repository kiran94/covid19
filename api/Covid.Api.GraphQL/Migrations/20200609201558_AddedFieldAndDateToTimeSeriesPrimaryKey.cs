using Microsoft.EntityFrameworkCore.Migrations;

namespace Covid.Api.GraphQL.Migrations
{
    public partial class AddedFieldAndDateToTimeSeriesPrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_timeseries",
                schema: "public",
                table: "timeseries");

            migrationBuilder.AlterColumn<string>(
                name: "field",
                schema: "public",
                table: "timeseries",
                type: "varchar",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar",
                oldMaxLength: 128,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_timeseries",
                schema: "public",
                table: "timeseries",
                columns: new[] { "country_region", "province_state", "county", "date", "field" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_timeseries",
                schema: "public",
                table: "timeseries");

            migrationBuilder.AlterColumn<string>(
                name: "field",
                schema: "public",
                table: "timeseries",
                type: "varchar",
                maxLength: 128,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar",
                oldMaxLength: 128);

            migrationBuilder.AddPrimaryKey(
                name: "pk_timeseries",
                schema: "public",
                table: "timeseries",
                columns: new[] { "country_region", "province_state", "county" });
        }
    }
}
