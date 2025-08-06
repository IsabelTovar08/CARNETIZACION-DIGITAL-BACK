using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurateUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                schema: "ModelSecurity",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PersonId",
                schema: "ModelSecurity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "ModelSecurity",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "ModelSecurity",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                schema: "ModelSecurity",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "ModelSecurity",
                table: "People",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                schema: "ModelSecurity",
                table: "Forms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 1,
                column: "ModuleId",
                value: 0);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 2,
                column: "ModuleId",
                value: 0);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 3,
                column: "ModuleId",
                value: 0);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Forms",
                keyColumn: "Id",
                keyValue: 4,
                column: "ModuleId",
                value: 0);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 2,
                column: "Email",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 3,
                column: "Email",
                value: null);

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "People",
                keyColumn: "Id",
                keyValue: 4,
                column: "Email",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PersonId",
                schema: "ModelSecurity",
                table: "Users",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                schema: "ModelSecurity",
                table: "Users",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_ModuleId",
                schema: "ModelSecurity",
                table: "Forms",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Modules_ModuleId",
                schema: "ModelSecurity",
                table: "Forms",
                column: "ModuleId",
                principalSchema: "ModelSecurity",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_Modules_ModuleId",
                schema: "ModelSecurity",
                table: "Forms");

            migrationBuilder.DropIndex(
                name: "IX_Users_PersonId",
                schema: "ModelSecurity",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserName",
                schema: "ModelSecurity",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Forms_ModuleId",
                schema: "ModelSecurity",
                table: "Forms");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "ModelSecurity",
                table: "People");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                schema: "ModelSecurity",
                table: "Forms");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                schema: "ModelSecurity",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                schema: "ModelSecurity",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "ModelSecurity",
                table: "Users",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Email",
                value: "funcionario@carnet.edu");

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                column: "Email",
                value: "laura.estudiante@correo.com");

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                column: "Email",
                value: "admin@carnet.edu");

            migrationBuilder.UpdateData(
                schema: "ModelSecurity",
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                column: "Email",
                value: "usuario@carnet.edu");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "ModelSecurity",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_PersonId",
                schema: "ModelSecurity",
                table: "Users",
                column: "PersonId",
                unique: true,
                filter: "[PersonId] IS NOT NULL");
        }
    }
}
