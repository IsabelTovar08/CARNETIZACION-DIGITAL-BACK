using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class menuv4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Modules",
                columns: new[] { "Id", "Description", "Icon", "Name" },
                values: new object[] { 5, "Dominio de seguridad", "admin_panel_settings", "Seguridad" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Modules",
                columns: new[] { "Id", "Description", "Icon", "Name" },
                values: new object[] { 9, "Dominio de seguridad", "admin_panel_settings", "Seguridad" });
        }
    }
}
