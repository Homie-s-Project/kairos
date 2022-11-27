using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Kairos.API.Migrations
{
    public partial class DatabaseTotalChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Events_EventId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Labels_Events_EventId",
                table: "Labels");

            migrationBuilder.DropTable(
                name: "GroupUser");

            migrationBuilder.DropIndex(
                name: "IX_Labels_EventId",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Studies");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "OwnerType",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "OwnerType",
                table: "Studies",
                newName: "GroupId");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Users",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Groups",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<bool>(
                name: "GroupsIsPrivate",
                table: "Groups",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CompanionS",
                columns: table => new
                {
                    CompanionId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanionName = table.Column<string>(type: "text", nullable: true),
                    CompanionType = table.Column<int>(type: "integer", nullable: false),
                    CompanionAge = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanionS", x => x.CompanionId);
                });

            migrationBuilder.CreateTable(
                name: "EventLabel",
                columns: table => new
                {
                    EventsEventId = table.Column<int>(type: "integer", nullable: false),
                    LabelsLabelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventLabel", x => new { x.EventsEventId, x.LabelsLabelId });
                    table.ForeignKey(
                        name: "FK_EventLabel_Events_EventsEventId",
                        column: x => x.EventsEventId,
                        principalTable: "Events",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventLabel_Labels_LabelsLabelId",
                        column: x => x.LabelsLabelId,
                        principalTable: "Labels",
                        principalColumn: "LabelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupLabel",
                columns: table => new
                {
                    GroupsGroupId = table.Column<int>(type: "integer", nullable: false),
                    LabelsLabelId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupLabel", x => new { x.GroupsGroupId, x.LabelsLabelId });
                    table.ForeignKey(
                        name: "FK_GroupLabel_Groups_GroupsGroupId",
                        column: x => x.GroupsGroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupLabel_Labels_LabelsLabelId",
                        column: x => x.LabelsLabelId,
                        principalTable: "Labels",
                        principalColumn: "LabelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ItemName = table.Column<string>(type: "text", nullable: true),
                    ItemType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.ItemId);
                });

            migrationBuilder.CreateTable(
                name: "LabelStudies",
                columns: table => new
                {
                    LabelsLabelId = table.Column<int>(type: "integer", nullable: false),
                    StudiesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelStudies", x => new { x.LabelsLabelId, x.StudiesId });
                    table.ForeignKey(
                        name: "FK_LabelStudies_Labels_LabelsLabelId",
                        column: x => x.LabelsLabelId,
                        principalTable: "Labels",
                        principalColumn: "LabelId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LabelStudies_Studies_StudiesId",
                        column: x => x.StudiesId,
                        principalTable: "Studies",
                        principalColumn: "StudiesId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanionItem",
                columns: table => new
                {
                    CompanionsCompanionId = table.Column<int>(type: "integer", nullable: false),
                    ItemsItemId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanionItem", x => new { x.CompanionsCompanionId, x.ItemsItemId });
                    table.ForeignKey(
                        name: "FK_CompanionItem_CompanionS_CompanionsCompanionId",
                        column: x => x.CompanionsCompanionId,
                        principalTable: "CompanionS",
                        principalColumn: "CompanionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanionItem_Items_ItemsItemId",
                        column: x => x.ItemsItemId,
                        principalTable: "Items",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_GroupId",
                table: "Users",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Studies_GroupId",
                table: "Studies",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UserId",
                table: "Groups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanionItem_ItemsItemId",
                table: "CompanionItem",
                column: "ItemsItemId");

            migrationBuilder.CreateIndex(
                name: "IX_EventLabel_LabelsLabelId",
                table: "EventLabel",
                column: "LabelsLabelId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupLabel_LabelsLabelId",
                table: "GroupLabel",
                column: "LabelsLabelId");

            migrationBuilder.CreateIndex(
                name: "IX_LabelStudies_StudiesId",
                table: "LabelStudies",
                column: "StudiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Events_EventId",
                table: "Groups",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Users_UserId",
                table: "Groups",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Studies_Groups_GroupId",
                table: "Studies",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Events_EventId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Users_UserId",
                table: "Groups");

            migrationBuilder.DropForeignKey(
                name: "FK_Studies_Groups_GroupId",
                table: "Studies");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "CompanionItem");

            migrationBuilder.DropTable(
                name: "EventLabel");

            migrationBuilder.DropTable(
                name: "GroupLabel");

            migrationBuilder.DropTable(
                name: "LabelStudies");

            migrationBuilder.DropTable(
                name: "CompanionS");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Users_GroupId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Studies_GroupId",
                table: "Studies");

            migrationBuilder.DropIndex(
                name: "IX_Groups_UserId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GroupsIsPrivate",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Groups");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                table: "Studies",
                newName: "OwnerType");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Studies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Labels",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Groups",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Groups",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OwnerType",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GroupUser",
                columns: table => new
                {
                    GroupsGroupId = table.Column<int>(type: "integer", nullable: false),
                    UsersUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUser", x => new { x.GroupsGroupId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_GroupUser_Groups_GroupsGroupId",
                        column: x => x.GroupsGroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUser_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Labels_EventId",
                table: "Labels",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUser_UsersUserId",
                table: "GroupUser",
                column: "UsersUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Events_EventId",
                table: "Groups",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Labels_Events_EventId",
                table: "Labels",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "EventId");
        }
    }
}
