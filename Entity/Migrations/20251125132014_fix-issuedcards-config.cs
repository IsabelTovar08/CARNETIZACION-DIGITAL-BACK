using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class fixissuedcardsconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardConfigurations_Schedules_SheduleId",
                schema: "Organizational",
                table: "CardConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuedCards_People_PersonId",
                schema: "Organizational",
                table: "IssuedCards");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuedCards_Profiles_ProfileId",
                schema: "Organizational",
                table: "IssuedCards");

            migrationBuilder.DropIndex(
                name: "IX_IssuedCards_PersonId_ProfileId_InternalDivisionId",
                schema: "Organizational",
                table: "IssuedCards");

            migrationBuilder.DropIndex(
                name: "IX_IssuedCards_ProfileId",
                schema: "Organizational",
                table: "IssuedCards");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                schema: "Organizational",
                table: "IssuedCards",
                newName: "SheduleId");

            migrationBuilder.RenameColumn(
                name: "SheduleId",
                schema: "Organizational",
                table: "CardConfigurations",
                newName: "ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_CardConfigurations_SheduleId",
                schema: "Organizational",
                table: "CardConfigurations",
                newName: "IX_CardConfigurations_ProfileId");

            migrationBuilder.AddColumn<int>(
                name: "ProfilesId",
                schema: "Organizational",
                table: "IssuedCards",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "Organizational",
                table: "CardConfigurations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "CardConfigurations",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Default Card");

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "IssuedCards",
                keyColumn: "Id",
                keyValue: 1,
                column: "ProfilesId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "IssuedCards",
                keyColumn: "Id",
                keyValue: 2,
                column: "ProfilesId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "IssuedCards",
                keyColumn: "Id",
                keyValue: 3,
                column: "ProfilesId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "IssuedCards",
                keyColumn: "Id",
                keyValue: 4,
                column: "ProfilesId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Organizational",
                table: "IssuedCards",
                keyColumn: "Id",
                keyValue: 5,
                column: "ProfilesId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_IssuedCards_PersonId",
                schema: "Organizational",
                table: "IssuedCards",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuedCards_ProfilesId",
                schema: "Organizational",
                table: "IssuedCards",
                column: "ProfilesId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuedCards_SheduleId_InternalDivisionId",
                schema: "Organizational",
                table: "IssuedCards",
                columns: new[] { "SheduleId", "InternalDivisionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CardConfigurations_Profiles_ProfileId",
                schema: "Organizational",
                table: "CardConfigurations",
                column: "ProfileId",
                principalSchema: "Organizational",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuedCards_People_PersonId",
                schema: "Organizational",
                table: "IssuedCards",
                column: "PersonId",
                principalSchema: "ModelSecurity",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuedCards_Profiles_ProfilesId",
                schema: "Organizational",
                table: "IssuedCards",
                column: "ProfilesId",
                principalSchema: "Organizational",
                principalTable: "Profiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_IssuedCards_Schedules_SheduleId",
                schema: "Organizational",
                table: "IssuedCards",
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
                name: "FK_CardConfigurations_Profiles_ProfileId",
                schema: "Organizational",
                table: "CardConfigurations");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuedCards_People_PersonId",
                schema: "Organizational",
                table: "IssuedCards");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuedCards_Profiles_ProfilesId",
                schema: "Organizational",
                table: "IssuedCards");

            migrationBuilder.DropForeignKey(
                name: "FK_IssuedCards_Schedules_SheduleId",
                schema: "Organizational",
                table: "IssuedCards");

            migrationBuilder.DropIndex(
                name: "IX_IssuedCards_PersonId",
                schema: "Organizational",
                table: "IssuedCards");

            migrationBuilder.DropIndex(
                name: "IX_IssuedCards_ProfilesId",
                schema: "Organizational",
                table: "IssuedCards");

            migrationBuilder.DropIndex(
                name: "IX_IssuedCards_SheduleId_InternalDivisionId",
                schema: "Organizational",
                table: "IssuedCards");

            migrationBuilder.DropColumn(
                name: "ProfilesId",
                schema: "Organizational",
                table: "IssuedCards");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "Organizational",
                table: "CardConfigurations");

            migrationBuilder.RenameColumn(
                name: "SheduleId",
                schema: "Organizational",
                table: "IssuedCards",
                newName: "ProfileId");

            migrationBuilder.RenameColumn(
                name: "ProfileId",
                schema: "Organizational",
                table: "CardConfigurations",
                newName: "SheduleId");

            migrationBuilder.RenameIndex(
                name: "IX_CardConfigurations_ProfileId",
                schema: "Organizational",
                table: "CardConfigurations",
                newName: "IX_CardConfigurations_SheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuedCards_PersonId_ProfileId_InternalDivisionId",
                schema: "Organizational",
                table: "IssuedCards",
                columns: new[] { "PersonId", "ProfileId", "InternalDivisionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IssuedCards_ProfileId",
                schema: "Organizational",
                table: "IssuedCards",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardConfigurations_Schedules_SheduleId",
                schema: "Organizational",
                table: "CardConfigurations",
                column: "SheduleId",
                principalSchema: "Organizational",
                principalTable: "Schedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuedCards_People_PersonId",
                schema: "Organizational",
                table: "IssuedCards",
                column: "PersonId",
                principalSchema: "ModelSecurity",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IssuedCards_Profiles_ProfileId",
                schema: "Organizational",
                table: "IssuedCards",
                column: "ProfileId",
                principalSchema: "Organizational",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
