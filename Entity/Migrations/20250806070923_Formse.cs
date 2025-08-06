using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class Formse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Forms_Modules_ModuleId",
                schema: "ModelSecurity",
                table: "Forms");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Modules_ModuleId",
                schema: "ModelSecurity",
                table: "Forms",
                column: "ModuleId",
                principalSchema: "ModelSecurity",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
