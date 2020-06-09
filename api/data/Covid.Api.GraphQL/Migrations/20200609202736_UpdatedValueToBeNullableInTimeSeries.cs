using Microsoft.EntityFrameworkCore.Migrations;

namespace Covid.Api.GraphQL.Migrations
{
    public partial class UpdatedValueToBeNullableInTimeSeries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "value",
                schema: "public",
                table: "timeseries",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "value",
                schema: "public",
                table: "timeseries",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);
        }
    }
}
