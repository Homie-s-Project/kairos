using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kairos.API.Migrations
{
    public partial class DatabaseUserCompanion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Companions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Companions_UserId",
                table: "Companions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companions_Users_UserId",
                table: "Companions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companions_Users_UserId",
                table: "Companions");

            migrationBuilder.DropIndex(
                name: "IX_Companions_UserId",
                table: "Companions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Companions");
        }
    }
}
