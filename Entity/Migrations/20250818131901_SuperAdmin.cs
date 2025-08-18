using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class SuperAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasAllPermissions",
                schema: "ModelSecurity",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "HasAllPermissions", "Name" },
                values: new object[] { "Acceso total al sistema.", true, "SuperAdmin" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "HasAllPermissions", "Name" },
                values: new object[] { "Administra carnets y eventos de su organización.", false, "OrgAdmin" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "HasAllPermissions", "Name" },
                values: new object[] { "Gestiona únicamente los eventos (creación, control y reportes).", false, "Supervisor" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "HasAllPermissions", "Name" },
                values: new object[] { "Funcionario (docentes, coordinadores, etc.) con visualización de su propio carnet.", false, "Administrativo" });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Roles",
                columns: new[] { "Id", "Description", "HasAllPermissions", "Name" },
                values: new object[,]
                {
                    { 5, "Consulta su propio carnet y asistencia.", false, "Estudiante" },
                    { 6, "Acceso mínimo/público.", false, "Usuario" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "HasAllPermissions",
                schema: "ModelSecurity",
                table: "Roles");

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Rol para personal autorizado a validar y emitir carnets", "Funcionario" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Rol con permisos limitados a visualización de carnet y asistencia", "Estudiante" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Acceso total al sistema de carnetización digital", "Admin" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Name" },
                values: new object[] { "Acceso restringido, solo visualización", "Usuario" });
        }
    }
}
