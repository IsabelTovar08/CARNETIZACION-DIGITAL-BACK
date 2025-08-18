using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class menustructurev2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                schema: "ModelSecurity",
                table: "MenuStructures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "ModelSecurity",
                table: "MenuStructures",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 4,
                column: "ModuleId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 5,
                column: "ModuleId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 6,
                column: "ModuleId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 7,
                column: "ModuleId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 8,
                column: "ModuleId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 9,
                column: "ModuleId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 10,
                column: "ModuleId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 11,
                column: "ModuleId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 12,
                column: "ModuleId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 13,
                column: "ModuleId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 14,
                column: "ModuleId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 15,
                column: "ModuleId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 16,
                column: "ModuleId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 17,
                column: "ModuleId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 18,
                column: "ModuleId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 19,
                column: "ModuleId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 20,
                column: "ModuleId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 21,
                column: "ModuleId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 22,
                column: "ModuleId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 23,
                column: "ModuleId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 24,
                column: "ModuleId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 25,
                column: "ModuleId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 26,
                column: "ModuleId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Icon", "ModuleId", "Title" },
                values: new object[] { null, 3, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Icon", "ModuleId", "Title" },
                values: new object[] { null, 3, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Icon", "ModuleId", "Title" },
                values: new object[] { null, 4, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Icon", "ModuleId", "Title" },
                values: new object[] { "account_tree", null, "Estructura Organizativa" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Icon", "ModuleId", "Title" },
                values: new object[] { "event_available", null, "Eventos y Control de Acceso" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Icon", "ModuleId", "Title" },
                values: new object[] { "settings_applications", null, "Configuración General" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Icon", "ModuleId", "Title" },
                values: new object[] { "location_on", null, "Ubicación" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Icon", "ModuleId", "Title" },
                values: new object[] { "admin_panel_settings", null, "Gestión de Seguridad" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "Icon", "Title" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Icon", "Name" },
                values: new object[] { "Dominio Operacional", "event_available", "Operacional" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Icon", "Name" },
                values: new object[] { "Parámetros y configuración", "settings_applications", "Parámetros" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Icon", "Name" },
                values: new object[] { "Dominio de seguridad", "admin_panel_settings", "Seguridad" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                schema: "ModelSecurity",
                table: "MenuStructures");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "ModelSecurity",
                table: "MenuStructures");

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 4,
                column: "ModuleId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 5,
                column: "ModuleId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 6,
                column: "ModuleId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 7,
                column: "ModuleId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 8,
                column: "ModuleId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 9,
                column: "ModuleId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 10,
                column: "ModuleId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 11,
                column: "ModuleId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 12,
                column: "ModuleId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 13,
                column: "ModuleId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 14,
                column: "ModuleId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 15,
                column: "ModuleId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 16,
                column: "ModuleId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 17,
                column: "ModuleId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 18,
                column: "ModuleId",
                value: 8);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 19,
                column: "ModuleId",
                value: 8);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 20,
                column: "ModuleId",
                value: 10);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 21,
                column: "ModuleId",
                value: 10);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 22,
                column: "ModuleId",
                value: 10);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 23,
                column: "ModuleId",
                value: 10);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 24,
                column: "ModuleId",
                value: 10);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 25,
                column: "ModuleId",
                value: 10);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 26,
                column: "ModuleId",
                value: 10);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 3,
                column: "ModuleId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 4,
                column: "ModuleId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 5,
                column: "ModuleId",
                value: 9);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 9,
                column: "ModuleId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 16,
                column: "ModuleId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 22,
                column: "ModuleId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 23,
                column: "ModuleId",
                value: 8);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "MenuStructures",
                keyColumn: "Id",
                keyValue: 29,
                column: "ModuleId",
                value: 10);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Icon", "Name" },
                values: new object[] { "Sección de estructura organizativa", "account_tree", "Estructura Organizativa" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Icon", "Name" },
                values: new object[] { "Dominio Operacional", "event_available", "Operacional" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Icon", "Name" },
                values: new object[] { "Eventos, accesos y asistencia", "event_available", "Eventos y Control de Acceso" });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Modules",
                columns: new[] { "Id", "Description", "Icon", "Name" },
                values: new object[,]
                {
                    { 6, "Parámetros y configuración", "settings_applications", "Parámetros" },
                    { 7, "Estados, tipos y categorías", "settings_applications", "Configuración General" },
                    { 8, "Departamentos y municipios", "location_on", "Ubicación" },
                    { 9, "Dominio de seguridad", "admin_panel_settings", "Seguridad" },
                    { 10, "Usuarios, roles y permisos", "admin_panel_settings", "Gestión de Seguridad" }
                });
        }
    }
}
