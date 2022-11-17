using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kairos.API.Migrations
{
    public partial class DatabaseRenameCompanion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanionItem_CompanionS_CompanionsCompanionId",
                table: "CompanionItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanionS",
                table: "CompanionS");

            migrationBuilder.RenameTable(
                name: "CompanionS",
                newName: "Companions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Companions",
                table: "Companions",
                column: "CompanionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanionItem_Companions_CompanionsCompanionId",
                table: "CompanionItem",
                column: "CompanionsCompanionId",
                principalTable: "Companions",
                principalColumn: "CompanionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanionItem_Companions_CompanionsCompanionId",
                table: "CompanionItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Companions",
                table: "Companions");

            migrationBuilder.RenameTable(
                name: "Companions",
                newName: "CompanionS");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanionS",
                table: "CompanionS",
                column: "CompanionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanionItem_CompanionS_CompanionsCompanionId",
                table: "CompanionItem",
                column: "CompanionsCompanionId",
                principalTable: "CompanionS",
                principalColumn: "CompanionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
