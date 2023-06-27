using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Application.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ExternalUserIdAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "externalid",
                schema: "administration",
                table: "portfoliousers",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "externalid",
                schema: "administration",
                table: "portfoliousers");
        }
    }
}
