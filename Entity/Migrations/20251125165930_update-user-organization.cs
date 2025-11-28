using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class updateuserorganization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrganizationId",
                schema: "ModelSecurity",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Description", "Icon", "ModuleId", "Name", "Url" },
                values: new object[] { "Mira y Crea tus propios diseños para tus estilos de carnet", "style", 4, "Diseños Carnets", "/dashboard/parametros/templates-available" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Description", "Icon", "Name", "Url" },
                values: new object[] { "Gestión de personas", "person_pin_circle", "Personas", "/dashboard/seguridad/people" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Description", "Icon", "Name", "Url" },
                values: new object[] { "Gestión de usuarios", "groups_2", "Usuarios", "/dashboard/seguridad/users" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Description", "Icon", "Name", "Url" },
                values: new object[] { "Gestión de roles", "add_moderator", "Roles", "/dashboard/seguridad/roles" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Description", "Icon", "Name", "Url" },
                values: new object[] { "Permisos por formulario", "folder_managed", "Gestión de Permisos", "/dashboard/seguridad/permission-forms" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Description", "Icon", "Name", "Url" },
                values: new object[] { "Catálogo de permisos", "lock_open_circle", "Permisos", "/dashboard/seguridad/permissions" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Description", "Icon", "Name", "Url" },
                values: new object[] { "Catálogo de formularios", "lists", "Formularios", "/dashboard/seguridad/forms" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Description", "Icon", "ModuleId", "Name", "Url" },
                values: new object[] { "Catálogo de módulos", "dashboard_2", 5, "Módulos", "/dashboard/seguridad/modules" });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Forms",
                columns: new[] { "Id", "Code", "CreateAt", "Description", "Icon", "ModuleId", "Name", "UpdateAt", "Url" },
                values: new object[,]
                {
                    { 25, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Consulta de generación masiva de carnets", "groups", 3, "Gestión de personas", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "/dashboard/operational/people-management" },
                    { 26, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "published_with_changes", 3, "Solicitudes de Modificación", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "/dashboard/operational/modification-request" }
                });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "OrganizationId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "OrganizationId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "OrganizationId",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "OrganizationId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                column: "OrganizationId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 6,
                column: "OrganizationId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 7,
                column: "OrganizationId",
                value: 3);

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrganizationId",
                schema: "ModelSecurity",
                table: "Users",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Organizations_OrganizationId",
                schema: "ModelSecurity",
                table: "Users",
                column: "OrganizationId",
                principalSchema: "Organizational",
                principalTable: "Organizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Organizations_OrganizationId",
                schema: "ModelSecurity",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_OrganizationId",
                schema: "ModelSecurity",
                table: "Users");

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "ModelSecurity",
                table: "Users");

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Description", "Icon", "ModuleId", "Name", "Url" },
                values: new object[] { "Gestión de personas", "person_pin_circle", 5, "Personas", "/dashboard/seguridad/people" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Description", "Icon", "Name", "Url" },
                values: new object[] { "Gestión de usuarios", "groups_2", "Usuarios", "/dashboard/seguridad/users" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Description", "Icon", "Name", "Url" },
                values: new object[] { "Gestión de roles", "add_moderator", "Roles", "/dashboard/seguridad/roles" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Description", "Icon", "Name", "Url" },
                values: new object[] { "Permisos por formulario", "folder_managed", "Gestión de Permisos", "/dashboard/seguridad/permission-forms" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Description", "Icon", "Name", "Url" },
                values: new object[] { "Catálogo de permisos", "lock_open_circle", "Permisos", "/dashboard/seguridad/permissions" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Description", "Icon", "Name", "Url" },
                values: new object[] { "Catálogo de formularios", "lists", "Formularios", "/dashboard/seguridad/forms" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Description", "Icon", "Name", "Url" },
                values: new object[] { "Catálogo de módulos", "dashboard_2", "Módulos", "/dashboard/seguridad/modules" });

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Description", "Icon", "ModuleId", "Name", "Url" },
                values: new object[] { "Consulta de generación masiva de carnets", "groups", 3, "Gestión de personas", "/dashboard/operational/people-management" });
        }
    }
}
