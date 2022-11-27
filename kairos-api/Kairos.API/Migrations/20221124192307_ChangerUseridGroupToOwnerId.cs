using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kairos.API.Migrations
{
    public partial class ChangerUseridGroupToOwnerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Groups",
                newName: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Groups",
                newName: "UserId");
        }
    }
}
