using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class Unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserRoles_RolId",
                schema: "ModelSecurity",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_RolFormPermissions_RolId",
                schema: "ModelSecurity",
                table: "RolFormPermissions");

            migrationBuilder.DropIndex(
                name: "IX_ModuleForms_ModuleId",
                schema: "ModelSecurity",
                table: "ModuleForms");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "ModelSecurity",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "ModelSecurity",
                table: "Roles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "ModelSecurity",
                table: "Permissions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Identification",
                schema: "ModelSecurity",
                table: "People",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "ModelSecurity",
                table: "Modules",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "ModelSecurity",
                table: "Forms",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "ModelSecurity",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RolId_UserId",
                schema: "ModelSecurity",
                table: "UserRoles",
                columns: new[] { "RolId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolFormPermissions_RolId_FormId_PermissionId",
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                columns: new[] { "RolId", "FormId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                schema: "ModelSecurity",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Name",
                schema: "ModelSecurity",
                table: "Permissions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_People_Identification",
                schema: "ModelSecurity",
                table: "People",
                column: "Identification",
                unique: true,
                filter: "[Identification] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Modules_Name",
                schema: "ModelSecurity",
                table: "Modules",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModuleForms_ModuleId_FormId",
                schema: "ModelSecurity",
                table: "ModuleForms",
                columns: new[] { "ModuleId", "FormId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Forms_Name",
                schema: "ModelSecurity",
                table: "Forms",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                schema: "ModelSecurity",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserRoles_RolId_UserId",
                schema: "ModelSecurity",
                table: "UserRoles");

            migrationBuilder.DropIndex(
                name: "IX_RolFormPermissions_RolId_FormId_PermissionId",
                schema: "ModelSecurity",
                table: "RolFormPermissions");

            migrationBuilder.DropIndex(
                name: "IX_Roles_Name",
                schema: "ModelSecurity",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_Name",
                schema: "ModelSecurity",
                table: "Permissions");

            migrationBuilder.DropIndex(
                name: "IX_People_Identification",
                schema: "ModelSecurity",
                table: "People");

            migrationBuilder.DropIndex(
                name: "IX_Modules_Name",
                schema: "ModelSecurity",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_ModuleForms_ModuleId_FormId",
                schema: "ModelSecurity",
                table: "ModuleForms");

            migrationBuilder.DropIndex(
                name: "IX_Forms_Name",
                schema: "ModelSecurity",
                table: "Forms");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                schema: "ModelSecurity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "ModelSecurity",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "ModelSecurity",
                table: "Permissions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Identification",
                schema: "ModelSecurity",
                table: "People",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "ModelSecurity",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "ModelSecurity",
                table: "Forms",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RolId",
                schema: "ModelSecurity",
                table: "UserRoles",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_RolFormPermissions_RolId",
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleForms_ModuleId",
                schema: "ModelSecurity",
                table: "ModuleForms",
                column: "ModuleId");
        }
    }
}
