using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity.Migrations.Postgres
{
    /// <inheritdoc />
    public partial class UpdateRelationInAttendance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AccessPoints_AccessPointOfEntry",
                schema: "Operational",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AccessPoints_AccessPointOfExit",
                schema: "Operational",
                table: "Attendances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventAccessPoints",
                schema: "Operational",
                table: "EventAccessPoints");

            migrationBuilder.DropColumn(
                name: "ScheduleDate",
                schema: "Operational",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ScheduleTime",
                schema: "Operational",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "QrCode",
                schema: "Operational",
                table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "AccessPointOfExit",
                schema: "Operational",
                table: "Attendances",
                newName: "EventId");

            migrationBuilder.RenameColumn(
                name: "AccessPointOfEntry",
                schema: "Operational",
                table: "Attendances",
                newName: "EventAccessPointExitId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_AccessPointOfExit",
                schema: "Operational",
                table: "Attendances",
                newName: "IX_Attendances_EventId");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_AccessPointOfEntry",
                schema: "Operational",
                table: "Attendances",
                newName: "IX_Attendances_EventAccessPointExitId");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Operational",
                table: "EventAccessPoints",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "AccessPointId",
                schema: "Operational",
                table: "Attendances",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccessPointId1",
                schema: "Operational",
                table: "Attendances",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EventAccessPointEntryId",
                schema: "Operational",
                table: "Attendances",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventAccessPoints",
                schema: "Operational",
                table: "EventAccessPoints",
                column: "Id");

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AccessPointId", "AccessPointId1", "EventAccessPointEntryId", "EventAccessPointExitId", "EventId" },
                values: new object[] { null, null, 1, 2, null });

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AccessPointId", "AccessPointId1", "EventAccessPointEntryId", "EventAccessPointExitId", "EventId" },
                values: new object[] { null, null, 1, 2, null });

            migrationBuilder.InsertData(
                schema: "Operational",
                table: "EventAccessPoints",
                columns: new[] { "Id", "AccessPointId", "Code", "CreateAt", "EventId", "IsDeleted", "UpdateAt" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 2, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventAccessPoints_EventId",
                schema: "Operational",
                table: "EventAccessPoints",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_AccessPointId",
                schema: "Operational",
                table: "Attendances",
                column: "AccessPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_AccessPointId1",
                schema: "Operational",
                table: "Attendances",
                column: "AccessPointId1");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_EventAccessPointEntryId",
                schema: "Operational",
                table: "Attendances",
                column: "EventAccessPointEntryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AccessPoints_AccessPointId",
                schema: "Operational",
                table: "Attendances",
                column: "AccessPointId",
                principalSchema: "Operational",
                principalTable: "AccessPoints",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AccessPoints_AccessPointId1",
                schema: "Operational",
                table: "Attendances",
                column: "AccessPointId1",
                principalSchema: "Operational",
                principalTable: "AccessPoints",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_EventAccessPoints_EventAccessPointEntryId",
                schema: "Operational",
                table: "Attendances",
                column: "EventAccessPointEntryId",
                principalSchema: "Operational",
                principalTable: "EventAccessPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_EventAccessPoints_EventAccessPointExitId",
                schema: "Operational",
                table: "Attendances",
                column: "EventAccessPointExitId",
                principalSchema: "Operational",
                principalTable: "EventAccessPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_Events_EventId",
                schema: "Operational",
                table: "Attendances",
                column: "EventId",
                principalSchema: "Operational",
                principalTable: "Events",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AccessPoints_AccessPointId",
                schema: "Operational",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_AccessPoints_AccessPointId1",
                schema: "Operational",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_EventAccessPoints_EventAccessPointEntryId",
                schema: "Operational",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_EventAccessPoints_EventAccessPointExitId",
                schema: "Operational",
                table: "Attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_Attendances_Events_EventId",
                schema: "Operational",
                table: "Attendances");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventAccessPoints",
                schema: "Operational",
                table: "EventAccessPoints");

            migrationBuilder.DropIndex(
                name: "IX_EventAccessPoints_EventId",
                schema: "Operational",
                table: "EventAccessPoints");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_AccessPointId",
                schema: "Operational",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_AccessPointId1",
                schema: "Operational",
                table: "Attendances");

            migrationBuilder.DropIndex(
                name: "IX_Attendances_EventAccessPointEntryId",
                schema: "Operational",
                table: "Attendances");

            migrationBuilder.DeleteData(
                schema: "Operational",
                table: "EventAccessPoints",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "Operational",
                table: "EventAccessPoints",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "AccessPointId",
                schema: "Operational",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "AccessPointId1",
                schema: "Operational",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "EventAccessPointEntryId",
                schema: "Operational",
                table: "Attendances");

            migrationBuilder.RenameColumn(
                name: "EventId",
                schema: "Operational",
                table: "Attendances",
                newName: "AccessPointOfExit");

            migrationBuilder.RenameColumn(
                name: "EventAccessPointExitId",
                schema: "Operational",
                table: "Attendances",
                newName: "AccessPointOfEntry");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_EventId",
                schema: "Operational",
                table: "Attendances",
                newName: "IX_Attendances_AccessPointOfExit");

            migrationBuilder.RenameIndex(
                name: "IX_Attendances_EventAccessPointExitId",
                schema: "Operational",
                table: "Attendances",
                newName: "IX_Attendances_AccessPointOfEntry");

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduleDate",
                schema: "Operational",
                table: "Events",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ScheduleTime",
                schema: "Operational",
                table: "Events",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                schema: "Operational",
                table: "EventAccessPoints",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "QrCode",
                schema: "Operational",
                table: "Attendances",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventAccessPoints",
                schema: "Operational",
                table: "EventAccessPoints",
                columns: new[] { "EventId", "AccessPointId" });

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AccessPointOfEntry", "AccessPointOfExit", "QrCode" },
                values: new object[] { 1, 2, null });

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "Attendances",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AccessPointOfEntry", "AccessPointOfExit", "QrCode" },
                values: new object[] { 1, 2, null });

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ScheduleDate", "ScheduleTime" },
                values: new object[] { new DateTime(2023, 7, 30, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1900, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                schema: "Operational",
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ScheduleDate", "ScheduleTime" },
                values: new object[] { new DateTime(2023, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1900, 1, 1, 9, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AccessPoints_AccessPointOfEntry",
                schema: "Operational",
                table: "Attendances",
                column: "AccessPointOfEntry",
                principalSchema: "Operational",
                principalTable: "AccessPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Attendances_AccessPoints_AccessPointOfExit",
                schema: "Operational",
                table: "Attendances",
                column: "AccessPointOfExit",
                principalSchema: "Operational",
                principalTable: "AccessPoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
