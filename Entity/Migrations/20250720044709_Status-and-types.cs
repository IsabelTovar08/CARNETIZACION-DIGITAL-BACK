using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class Statusandtypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                table: "Notifications");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "AccessPointId",
                table: "Events",
                newName: "StatusId");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                schema: "ModelSecurity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                schema: "ModelSecurity",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResetCode",
                schema: "ModelSecurity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetCodeExpiration",
                schema: "ModelSecurity",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Organizations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AccessPointTypeId",
                table: "AccessPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "AccessPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AccessPoint",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomTypeId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessPoint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessPoint_CustomTypes_CustomTypeId",
                        column: x => x.CustomTypeId,
                        principalTable: "CustomTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "RefreshToken", "RefreshTokenExpiryTime", "ResetCode", "ResetCodeExpiration" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "RefreshToken", "RefreshTokenExpiryTime", "ResetCode", "ResetCodeExpiration" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "RefreshToken", "RefreshTokenExpiryTime", "ResetCode", "ResetCodeExpiration" },
                values: new object[] { null, null, null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "RefreshToken", "RefreshTokenExpiryTime", "ResetCode", "ResetCodeExpiration" },
                values: new object[] { null, null, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_People_DocumenTypeId",
                schema: "ModelSecurity",
                table: "People",
                column: "DocumenTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_OrganizaionTypeId",
                table: "Organizations",
                column: "OrganizaionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_StatusId",
                table: "Events",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_StatusId",
                table: "Cards",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPoints_AccessPointTypeId",
                table: "AccessPoints",
                column: "AccessPointTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPoint_CustomTypeId",
                table: "AccessPoint",
                column: "CustomTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccessPoints_CustomTypes_AccessPointTypeId",
                table: "AccessPoints",
                column: "AccessPointTypeId",
                principalTable: "CustomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Statuses_StatusId",
                table: "Cards",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Statuses_StatusId",
                table: "Events",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_CustomTypes_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId",
                principalTable: "CustomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Organizations_CustomTypes_OrganizaionTypeId",
                table: "Organizations",
                column: "OrganizaionTypeId",
                principalTable: "CustomTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_People_CustomTypes_DocumenTypeId",
                schema: "ModelSecurity",
                table: "People",
                column: "DocumenTypeId",
                principalTable: "CustomTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccessPoints_CustomTypes_AccessPointTypeId",
                table: "AccessPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Statuses_StatusId",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Statuses_StatusId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_CustomTypes_NotificationTypeId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Organizations_CustomTypes_OrganizaionTypeId",
                table: "Organizations");

            migrationBuilder.DropForeignKey(
                name: "FK_People_CustomTypes_DocumenTypeId",
                schema: "ModelSecurity",
                table: "People");

            migrationBuilder.DropTable(
                name: "AccessPoint");

            migrationBuilder.DropIndex(
                name: "IX_People_DocumenTypeId",
                schema: "ModelSecurity",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_Organizations_OrganizaionTypeId",
                table: "Organizations");

            migrationBuilder.DropIndex(
                name: "IX_Events_StatusId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Cards_StatusId",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_AccessPoints_AccessPointTypeId",
                table: "AccessPoints");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                schema: "ModelSecurity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                schema: "ModelSecurity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetCode",
                schema: "ModelSecurity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetCodeExpiration",
                schema: "ModelSecurity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "AccessPointTypeId",
                table: "AccessPoints");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "AccessPoints");

            migrationBuilder.RenameColumn(
                name: "StatusId",
                table: "Events",
                newName: "AccessPointId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Events",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId",
                principalTable: "NotificationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
