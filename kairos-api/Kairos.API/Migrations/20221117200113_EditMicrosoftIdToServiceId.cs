using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kairos.API.Migrations
{
    public partial class EditMicrosoftIdToServiceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MicrosoftId",
                table: "Users",
                newName: "ServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "Users",
                newName: "MicrosoftId");
        }
    }
}
