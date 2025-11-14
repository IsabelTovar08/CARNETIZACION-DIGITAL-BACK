using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class UpdateRelationInEventAttendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventAccessPoints_Events_EventId",
                schema: "Operational",
                table: "EventAccessPoints");

            migrationBuilder.AddForeignKey(
                name: "FK_EventAccessPoints_Events_EventId",
                schema: "Operational",
                table: "EventAccessPoints",
                column: "EventId",
                principalSchema: "Operational",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventAccessPoints_Events_EventId",
                schema: "Operational",
                table: "EventAccessPoints");

            migrationBuilder.AddForeignKey(
                name: "FK_EventAccessPoints_Events_EventId",
                schema: "Operational",
                table: "EventAccessPoints",
                column: "EventId",
                principalSchema: "Operational",
                principalTable: "Events",
                principalColumn: "Id");
        }
    }
}