using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolFormPermission_Forms_FormId",
                table: "RolFormPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RolFormPermission_Permissions_PermissionId",
                table: "RolFormPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_RolFormPermission_Roles_RolId",
                table: "RolFormPermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolFormPermission",
                table: "RolFormPermission");

            migrationBuilder.EnsureSchema(
                name: "ModelSecurity");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "Users",
                newSchema: "ModelSecurity");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                newName: "UserRoles",
                newSchema: "ModelSecurity");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Roles",
                newSchema: "ModelSecurity");

            migrationBuilder.RenameTable(
                name: "Permissions",
                newName: "Permissions",
                newSchema: "ModelSecurity");

            migrationBuilder.RenameTable(
                name: "People",
                newName: "People",
                newSchema: "ModelSecurity");

            migrationBuilder.RenameTable(
                name: "Modules",
                newName: "Modules",
                newSchema: "ModelSecurity");

            migrationBuilder.RenameTable(
                name: "ModuleForms",
                newName: "ModuleForms",
                newSchema: "ModelSecurity");

            migrationBuilder.RenameTable(
                name: "Forms",
                newName: "Forms",
                newSchema: "ModelSecurity");

            migrationBuilder.RenameTable(
                name: "RolFormPermission",
                newName: "RolFormPermissions",
                newSchema: "ModelSecurity");

            migrationBuilder.RenameIndex(
                name: "IX_RolFormPermission_RolId",
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                newName: "IX_RolFormPermissions_RolId");

            migrationBuilder.RenameIndex(
                name: "IX_RolFormPermission_PermissionId",
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                newName: "IX_RolFormPermissions_PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_RolFormPermission_FormId",
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                newName: "IX_RolFormPermissions_FormId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "ModelSecurity",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "ModelSecurity",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "ModelSecurity",
                table: "People",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "ModelSecurity",
                table: "Modules",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "ModelSecurity",
                table: "ModuleForms",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "ModelSecurity",
                table: "Forms",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolFormPermissions",
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                column: "Id");

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Forms",
                columns: new[] { "Id", "Description", "Name", "Url" },
                values: new object[,]
                {
                    { 1, "Formulario para generar un nuevo carnet digital", "Crear Carnet", "/formulario" },
                    { 2, "Formulario para validar el correo del usuario", "Validar Correo", "/formulario" },
                    { 3, "Formulario donde se visualiza el carnet", "Ver Carnet", "/formulario" },
                    { 4, "Formulario para registrar y consultar asistencia", "Control de Asistencia", "/formulario" }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Modules",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Gestión y emisión de carnets digitales", "Carnetización" },
                    { 2, "Validación de identidad y correos", "Validación" },
                    { 3, "Módulo para control de asistencia en eventos/clases", "Asistencia" }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "People",
                columns: new[] { "Id", "FirstName", "Identification", "LastName", "MiddleName", "Phone", "SecondLastName" },
                values: new object[,]
                {
                    { 1, "Carlos", "1234567890", "Funcionario", null, "3200001111", null },
                    { 2, "Laura", "9876543210", "Estudiante", null, "3100002222", null },
                    { 3, "Ana", "1122334455", "Administrador", null, "3001234567", null },
                    { 4, "José", "9988776655", "Usuario", null, "3151234567", null }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Permissions",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Puede crear nuevos registros", "crear" },
                    { 2, "Puede editar registros existentes", "editar" },
                    { 3, "Puede validar datos (correo, QR)", "validar" }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Roles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Rol para personal autorizado a validar y emitir carnets", "Funcionario" },
                    { 2, "Rol con permisos limitados a visualización de carnet y asistencia", "Estudiante" },
                    { 3, "Acceso total al sistema de carnetización digital", "Admin" },
                    { 4, "Acceso restringido, solo visualización", "Usuario" }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "ModuleForms",
                columns: new[] { "Id", "FormId", "ModuleId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 2 },
                    { 3, 3, 3 },
                    { 4, 4, 3 }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                columns: new[] { "Id", "FormId", "PermissionId", "RolId" },
                values: new object[,]
                {
                    { 1, 1, 1, 3 },
                    { 2, 2, 3, 3 },
                    { 3, 3, 2, 3 }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Users",
                columns: new[] { "Id", "DateCreated", "Email", "IsDeleted", "Password", "PersonId", "UserName" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "funcionario@carnet.edu", false, "123", 1, "admin" },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "laura.estudiante@correo.com", false, "L4d!Estudiante2025", 2, null },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@carnet.edu", false, "Adm!nCarnet2025", 3, null },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "usuario@carnet.edu", false, "Usr!Carnet2025", 4, null }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "UserRoles",
                columns: new[] { "Id", "IsDeleted", "RolId", "UserId" },
                values: new object[,]
                {
                    { 1, false, 1, 1 },
                    { 2, false, 2, 2 },
                    { 3, false, 3, 3 },
                    { 4, false, 4, 4 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_RolFormPermissions_Forms_FormId",
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                column: "FormId",
                principalSchema: "ModelSecurity",
                principalTable: "Forms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolFormPermissions_Permissions_PermissionId",
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                column: "PermissionId",
                principalSchema: "ModelSecurity",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolFormPermissions_Roles_RolId",
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                column: "RolId",
                principalSchema: "ModelSecurity",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolFormPermissions_Forms_FormId",
                schema: "ModelSecurity",
                table: "RolFormPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolFormPermissions_Permissions_PermissionId",
                schema: "ModelSecurity",
                table: "RolFormPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_RolFormPermissions_Roles_RolId",
                schema: "ModelSecurity",
                table: "RolFormPermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RolFormPermissions",
                schema: "ModelSecurity",
                table: "RolFormPermissions");

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "ModuleForms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "ModuleForms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "ModuleForms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "ModuleForms",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Modules",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "ModelSecurity",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "UserRoles",
                schema: "ModelSecurity",
                newName: "UserRoles");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "ModelSecurity",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Permissions",
                schema: "ModelSecurity",
                newName: "Permissions");

            migrationBuilder.RenameTable(
                name: "People",
                schema: "ModelSecurity",
                newName: "People");

            migrationBuilder.RenameTable(
                name: "Modules",
                schema: "ModelSecurity",
                newName: "Modules");

            migrationBuilder.RenameTable(
                name: "ModuleForms",
                schema: "ModelSecurity",
                newName: "ModuleForms");

            migrationBuilder.RenameTable(
                name: "Forms",
                schema: "ModelSecurity",
                newName: "Forms");

            migrationBuilder.RenameTable(
                name: "RolFormPermissions",
                schema: "ModelSecurity",
                newName: "RolFormPermission");

            migrationBuilder.RenameIndex(
                name: "IX_RolFormPermissions_RolId",
                table: "RolFormPermission",
                newName: "IX_RolFormPermission_RolId");

            migrationBuilder.RenameIndex(
                name: "IX_RolFormPermissions_PermissionId",
                table: "RolFormPermission",
                newName: "IX_RolFormPermission_PermissionId");

            migrationBuilder.RenameIndex(
                name: "IX_RolFormPermissions_FormId",
                table: "RolFormPermission",
                newName: "IX_RolFormPermission_FormId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Roles",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Permissions",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "People",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Modules",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ModuleForms",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Forms",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "RolFormPermission",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RolFormPermission",
                table: "RolFormPermission",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RolFormPermission_Forms_FormId",
                table: "RolFormPermission",
                column: "FormId",
                principalTable: "Forms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolFormPermission_Permissions_PermissionId",
                table: "RolFormPermission",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RolFormPermission_Roles_RolId",
                table: "RolFormPermission",
                column: "RolId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
