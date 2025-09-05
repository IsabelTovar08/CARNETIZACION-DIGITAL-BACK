using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class code : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "ModelSecurity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "ModelSecurity",
                table: "UserRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Parameter",
                table: "TypeCategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Parameter",
                table: "Statuses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Organizational",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "ModelSecurity",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Auth",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Organizational",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Organizational",
                table: "PersonDivisionProfiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "ModelSecurity",
                table: "Permissions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "ModelSecurity",
                table: "People",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Organizational",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Organizational",
                table: "OrganizationalUnits",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Organizational",
                table: "OrganizationalUnitBranches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Notifications",
                table: "NotificationReceived",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Notifications",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "ModelSecurity",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Parameter",
                table: "MenuStructures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Organizational",
                table: "InternalDivisions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "ModelSecurity",
                table: "Forms",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Operational",
                table: "EventTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Organizational",
                table: "EventTargetAudience",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Organizational",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Parameter",
                table: "CustomTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Organizational",
                table: "Cities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Organizational",
                table: "Cards",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Organizational",
                table: "Branches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Operational",
                table: "Attendances",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Organizational",
                table: "Areas",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                schema: "Operational",
                table: "AccessPoints",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "AccessPoints",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "AccessPoints",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "AccessPoints",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Areas",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Areas",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Areas",
                keyColumn: "Id",
                keyValue: 4,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Areas",
                keyColumn: "Id",
                keyValue: 5,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Branches",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Branches",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Cards",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Cities",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Cities",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Cities",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 8,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 9,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 10,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 11,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 12,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 13,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 14,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 15,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 16,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 17,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 18,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 19,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 20,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 21,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 22,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 23,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 24,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 25,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 26,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 27,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "CustomTypes",
                keyColumn: "Id",
                keyValue: 28,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "EventTargetAudience",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "EventTargetAudience",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "EventTargetAudience",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "EventTypes",
                keyColumn: "Id",
                keyValue: 7,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 4,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 5,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 6,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 7,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 8,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 9,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 10,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 11,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 12,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 13,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 14,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 15,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 16,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 17,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 18,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 19,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 20,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 21,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 22,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 23,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 24,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "InternalDivisions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "InternalDivisions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "InternalDivisions",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "InternalDivisions",
                keyColumn: "Id",
                keyValue: 4,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "InternalDivisions",
                keyColumn: "Id",
                keyValue: 5,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 4,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 5,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 6,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 7,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 8,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 9,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 10,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 11,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 12,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 13,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 14,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 15,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 16,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 17,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 18,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 19,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 20,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 21,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 22,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 23,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 24,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 25,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 26,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 27,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 28,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 29,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 30,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 31,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 32,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 33,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 34,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 4,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 5,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Notifications",
                table: "Notification",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Notifications",
                table: "Notification",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Notifications",
                table: "NotificationReceived",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Notifications",
                table: "NotificationReceived",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "OrganizationalUnitBranches",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "OrganizationalUnitBranches",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "OrganizationalUnitBranches",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "OrganizationalUnitBranches",
                keyColumn: "Id",
                keyValue: 4,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "OrganizationalUnits",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "OrganizationalUnits",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "OrganizationalUnits",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Organizations",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Organizations",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Organizations",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 4,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 5,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 6,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 7,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "PersonDivisionProfiles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "PersonDivisionProfiles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 4,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Profiles",
                keyColumn: "Id",
                keyValue: 5,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 4,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 5,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 6,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 7,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 8,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 9,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 10,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 11,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "Statuses",
                keyColumn: "Id",
                keyValue: 12,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "TypeCategories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "TypeCategories",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "TypeCategories",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "TypeCategories",
                keyColumn: "Id",
                keyValue: 4,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "TypeCategories",
                keyColumn: "Id",
                keyValue: 5,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "TypeCategories",
                keyColumn: "Id",
                keyValue: 6,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 5,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 6,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 7,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "Code",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "Code",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                schema: "ModelSecurity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "ModelSecurity",
                table: "UserRoles");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Parameter",
                table: "TypeCategories");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Parameter",
                table: "Statuses");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Organizational",
                table: "Schedules");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "ModelSecurity",
                table: "RolFormPermissions");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "ModelSecurity",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Auth",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Organizational",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Organizational",
                table: "PersonDivisionProfiles");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "ModelSecurity",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "ModelSecurity",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Organizational",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Organizational",
                table: "OrganizationalUnits");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Organizational",
                table: "OrganizationalUnitBranches");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Notifications",
                table: "NotificationReceived");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Notifications",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "ModelSecurity",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Parameter",
                table: "MenuStructures");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Organizational",
                table: "InternalDivisions");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "ModelSecurity",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Operational",
                table: "EventTypes");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Organizational",
                table: "EventTargetAudience");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Organizational",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Parameter",
                table: "CustomTypes");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Organizational",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Organizational",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Organizational",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Operational",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Organizational",
                table: "Areas");

            migrationBuilder.DropColumn(
                name: "Code",
                schema: "Operational",
                table: "AccessPoints");
        }
    }
}
