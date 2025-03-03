using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Identity.Data.Migrations
{
    /// <inheritdoc />
    public partial class user_active_last_login : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "Identity",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                schema: "Identity",
                table: "Users",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "Identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                schema: "Identity",
                table: "Users");
        }
    }
}
