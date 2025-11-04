using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class AutoInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportBatchRows_IssuedCards_IssuedCardId",
                table: "ImportBatchRows");

            migrationBuilder.AlterColumn<int>(
                name: "IssuedCardId",
                table: "ImportBatchRows",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_ImportBatchRows_PersonId",
                table: "ImportBatchRows",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportBatchRows_IssuedCards_IssuedCardId",
                table: "ImportBatchRows",
                column: "IssuedCardId",
                principalSchema: "Organizational",
                principalTable: "IssuedCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportBatchRows_People_PersonId",
                table: "ImportBatchRows",
                column: "PersonId",
                principalSchema: "ModelSecurity",
                principalTable: "People",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportBatchRows_IssuedCards_IssuedCardId",
                table: "ImportBatchRows");

            migrationBuilder.DropForeignKey(
                name: "FK_ImportBatchRows_People_PersonId",
                table: "ImportBatchRows");

            migrationBuilder.DropIndex(
                name: "IX_ImportBatchRows_PersonId",
                table: "ImportBatchRows");

            migrationBuilder.AlterColumn<int>(
                name: "IssuedCardId",
                table: "ImportBatchRows",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ImportBatchRows_IssuedCards_IssuedCardId",
                table: "ImportBatchRows",
                column: "IssuedCardId",
                principalSchema: "Organizational",
                principalTable: "IssuedCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
