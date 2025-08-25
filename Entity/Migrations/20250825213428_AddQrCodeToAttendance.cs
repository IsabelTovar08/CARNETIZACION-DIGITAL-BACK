using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class AddQrCodeToAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QrCode",
                schema: "Operational",
                table: "Attendances",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 1,
                column: "QrCode",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 2,
                column: "QrCode",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QrCode",
                schema: "Operational",
                table: "Attendances");
        }
    }
}
