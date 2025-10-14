using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class Importbatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SheduleId",
                schema: "Organizational",
                table: "Cards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "Cards",
                keyColumn: "Id",
                keyValue: 1,
                column: "SheduleId",
                value: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_SheduleId",
                schema: "Organizational",
                table: "Cards",
                column: "SheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cards_Schedules_SheduleId",
                schema: "Organizational",
                table: "Cards",
                column: "SheduleId",
                principalSchema: "Organizational",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cards_Schedules_SheduleId",
                schema: "Organizational",
                table: "Cards");

            migrationBuilder.DropIndex(
                name: "IX_Cards_SheduleId",
                schema: "Organizational",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "SheduleId",
                schema: "Organizational",
                table: "Cards");
        }
    }
}
