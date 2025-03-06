using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Identity.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_jwt_store_fk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_JwtStores_Users_UserId",
                schema: "Identity",
                table: "JwtStores",
                column: "UserId",
                principalSchema: "Identity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JwtStores_Users_UserId",
                schema: "Identity",
                table: "JwtStores");
        }
    }
}
