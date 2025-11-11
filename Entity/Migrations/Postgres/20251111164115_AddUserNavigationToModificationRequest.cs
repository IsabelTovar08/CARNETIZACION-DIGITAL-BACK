using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AddUserNavigationToModificationRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_modificationRequests_UserId",
                table: "modificationRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_modificationRequests_Users_UserId",
                table: "modificationRequests",
                column: "UserId",
                principalSchema: "ModelSecurity",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_modificationRequests_Users_UserId",
                table: "modificationRequests");

            migrationBuilder.DropIndex(
                name: "IX_modificationRequests_UserId",
                table: "modificationRequests");
        }
    }
}
