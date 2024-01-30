using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stock.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_Deactivation_Attributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "modifiedby",
                table: "stocks_prices",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "createdby",
                table: "stocks_prices",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "deactivated",
                table: "stocks_prices",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isactive",
                table: "stocks_prices",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "deactivated",
                table: "stocks",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isactive",
                table: "stocks",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deactivated",
                table: "stocks_prices");

            migrationBuilder.DropColumn(
                name: "isactive",
                table: "stocks_prices");

            migrationBuilder.DropColumn(
                name: "deactivated",
                table: "stocks");

            migrationBuilder.DropColumn(
                name: "isactive",
                table: "stocks");

            migrationBuilder.AlterColumn<string>(
                name: "modifiedby",
                table: "stocks_prices",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "createdby",
                table: "stocks_prices",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
