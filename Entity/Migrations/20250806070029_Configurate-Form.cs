using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurateForm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModuleForms",
                schema: "ModelSecurity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModuleForms",
                schema: "ModelSecurity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleForms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleForms_Forms_FormId",
                        column: x => x.FormId,
                        principalSchema: "ModelSecurity",
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModuleForms_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "ModelSecurity",
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_ModuleForms_FormId",
                schema: "ModelSecurity",
                table: "ModuleForms",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleForms_ModuleId_FormId",
                schema: "ModelSecurity",
                table: "ModuleForms",
                columns: new[] { "ModuleId", "FormId" },
                unique: true);
        }
    }
}
