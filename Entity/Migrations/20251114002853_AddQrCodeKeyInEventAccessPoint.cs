using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AddQrCodeKeyInEventAccessPoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QrCodeKey",
                schema: "Operational",
                table: "EventAccessPoints",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "EventAccessPoints",
                keyColumn: "Id",
                keyValue: 1,
                column: "QrCodeKey",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "EventAccessPoints",
                keyColumn: "Id",
                keyValue: 2,
                column: "QrCodeKey",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QrCodeKey",
                schema: "Operational",
                table: "EventAccessPoints");
        }
    }
}
