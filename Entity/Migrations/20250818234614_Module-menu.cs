using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class Modulemenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 4,
                column: "ModuleId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 5,
                column: "ModuleId",
                value: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 4,
                column: "ModuleId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "Parameter",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 5,
                column: "ModuleId",
                value: 4);
        }
    }
}
