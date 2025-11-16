using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AddEntityEventShedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Schedules_ScheduleId",
                schema: "Operational",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_ScheduleId",
                schema: "Operational",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                schema: "Operational",
                table: "Events");

            migrationBuilder.CreateTable(
                name: "EventSchedules",
                schema: "Operational",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    ScheduleId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSchedules", x => new { x.EventId, x.ScheduleId });
                    table.ForeignKey(
                        name: "FK_EventSchedules_Events_EventId",
                        column: x => x.EventId,
                        principalSchema: "Operational",
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSchedules_Schedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalSchema: "Organizational",
                        principalTable: "Schedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "Operational",
                table: "EventSchedules",
                columns: new[] { "EventId", "ScheduleId", "Code", "CreateAt", "Id", "IsDeleted", "UpdateAt" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 1, 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 3, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventSchedules_ScheduleId",
                schema: "Operational",
                table: "EventSchedules",
                column: "ScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventSchedules",
                schema: "Operational");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                schema: "Operational",
                table: "Events",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "ScheduleId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                column: "ScheduleId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Events_ScheduleId",
                schema: "Operational",
                table: "Events",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Schedules_ScheduleId",
                schema: "Operational",
                table: "Events",
                column: "ScheduleId",
                principalSchema: "Organizational",
                principalTable: "Schedules",
                principalColumn: "Id");
        }
    }
}
