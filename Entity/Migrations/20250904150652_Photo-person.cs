using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class Photoperson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Photo",
                schema: "ModelSecurity",
                table: "People",
                newName: "PhotoUrl");

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                schema: "ModelSecurity",
                table: "People",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 1,
                column: "PhotoPath",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 2,
                column: "PhotoPath",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 3,
                column: "PhotoPath",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 4,
                column: "PhotoPath",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 5,
                column: "PhotoPath",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 6,
                column: "PhotoPath",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 7,
                column: "PhotoPath",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                schema: "ModelSecurity",
                table: "People");

            migrationBuilder.RenameColumn(
                name: "PhotoUrl",
                schema: "ModelSecurity",
                table: "People",
                newName: "Photo");
        }
    }
}
