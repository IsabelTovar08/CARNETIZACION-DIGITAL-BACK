using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AddNewFieldsinSheduleAndEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Days",
                schema: "Operational",
                table: "Events",
                newName: "QrCodeBase64");

            migrationBuilder.AddColumn<string>(
                name: "Days",
                schema: "Organizational",
                table: "Schedules",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 1,
                column: "Days",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 2,
                column: "Days",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Schedules",
                keyColumn: "Id",
                keyValue: 3,
                column: "Days",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Days",
                schema: "Organizational",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "QrCodeBase64",
                schema: "Operational",
                table: "Events",
                newName: "Days");
        }
    }
}
