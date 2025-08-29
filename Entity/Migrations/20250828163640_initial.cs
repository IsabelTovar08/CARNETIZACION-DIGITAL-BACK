using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Operational");

            migrationBuilder.EnsureSchema(
                name: "Organizational");

            migrationBuilder.EnsureSchema(
                name: "Parameter");

            migrationBuilder.EnsureSchema(
                name: "ModelSecurity");

            migrationBuilder.EnsureSchema(
                name: "Notifications");

            migrationBuilder.EnsureSchema(
                name: "Auth");

            migrationBuilder.CreateTable(
                name: "Areas",
                schema: "Organizational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "Organizational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventTypes",
                schema: "Operational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Modules",
                schema: "ModelSecurity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                schema: "ModelSecurity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                schema: "Organizational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                schema: "Auth",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TokenHash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReplacedByTokenHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "ModelSecurity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasAllPermissions = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statuses",
                schema: "Parameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeCategories",
                schema: "Parameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                schema: "Organizational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeparmentId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalSchema: "Organizational",
                        principalTable: "Departments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Forms",
                schema: "ModelSecurity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    ModuleId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forms_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "ModelSecurity",
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CustomTypes",
                schema: "Parameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TypeCategoryId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomTypes_TypeCategories_TypeCategoryId",
                        column: x => x.TypeCategoryId,
                        principalSchema: "Parameter",
                        principalTable: "TypeCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuStructures",
                schema: "Parameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentMenuId = table.Column<int>(type: "int", nullable: true),
                    ModuleId = table.Column<int>(type: "int", nullable: true),
                    FormId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuStructures", x => x.Id);
                    table.CheckConstraint("CK_MenuStructures_Type", "[Type] IN ('group','collapse','item')");
                    table.ForeignKey(
                        name: "FK_MenuStructures_Forms_FormId",
                        column: x => x.FormId,
                        principalSchema: "ModelSecurity",
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MenuStructures_MenuStructures_ParentMenuId",
                        column: x => x.ParentMenuId,
                        principalSchema: "Parameter",
                        principalTable: "MenuStructures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MenuStructures_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalSchema: "ModelSecurity",
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolFormPermissions",
                schema: "ModelSecurity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    FormId = table.Column<int>(type: "int", nullable: false),
                    PermissionId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolFormPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolFormPermissions_Forms_FormId",
                        column: x => x.FormId,
                        principalSchema: "ModelSecurity",
                        principalTable: "Forms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolFormPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "ModelSecurity",
                        principalTable: "Permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolFormPermissions_Roles_RolId",
                        column: x => x.RolId,
                        principalSchema: "ModelSecurity",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    NotificationTypeId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_CustomTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalSchema: "Parameter",
                        principalTable: "CustomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                schema: "Organizational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Organizations_CustomTypes_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "Parameter",
                        principalTable: "CustomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "People",
                schema: "ModelSecurity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondLastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentNumber = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentTypeId = table.Column<int>(type: "int", nullable: true),
                    BloodTypeId = table.Column<int>(type: "int", nullable: true),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_Cities_CityId",
                        column: x => x.CityId,
                        principalSchema: "Organizational",
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_People_CustomTypes_BloodTypeId",
                        column: x => x.BloodTypeId,
                        principalSchema: "Parameter",
                        principalTable: "CustomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_People_CustomTypes_DocumentTypeId",
                        column: x => x.DocumentTypeId,
                        principalSchema: "Parameter",
                        principalTable: "CustomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                schema: "Organizational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_Cities_CityId",
                        column: x => x.CityId,
                        principalSchema: "Organizational",
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Branches_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "Organizational",
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Schedules",
                schema: "Organizational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedules_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "Organizational",
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "ModelSecurity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResetCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetCodeExpiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TempCodeHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TempCodeCreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TempCodeExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TempCodeAttempts = table.Column<int>(type: "int", nullable: false),
                    TempCodeConsumedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TempCodeResendBlockedUntil = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_People_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "ModelSecurity",
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationalUnits",
                schema: "Organizational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationalUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationalUnits_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Organizational",
                        principalTable: "Branches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Events",
                schema: "Operational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScheduleDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ScheduleTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EventStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EventEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SheduleId = table.Column<int>(type: "int", nullable: true),
                    IsPublic = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    EventTypeId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_EventTypes_EventTypeId",
                        column: x => x.EventTypeId,
                        principalSchema: "Operational",
                        principalTable: "EventTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Schedules_SheduleId",
                        column: x => x.SheduleId,
                        principalSchema: "Organizational",
                        principalTable: "Schedules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Events_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "Parameter",
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NotificationReceived",
                schema: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReadDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationReceived", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationReceived_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalSchema: "Notifications",
                        principalTable: "Notification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NotificationReceived_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "Parameter",
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationReceived_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "ModelSecurity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                schema: "ModelSecurity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RolId",
                        column: x => x.RolId,
                        principalSchema: "ModelSecurity",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "ModelSecurity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InternalDivisions",
                schema: "Organizational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationalUnitId = table.Column<int>(type: "int", nullable: false),
                    AreaCategoryId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalDivisions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InternalDivisions_Areas_AreaCategoryId",
                        column: x => x.AreaCategoryId,
                        principalSchema: "Organizational",
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InternalDivisions_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Organizational",
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InternalDivisions_OrganizationalUnits_OrganizationalUnitId",
                        column: x => x.OrganizationalUnitId,
                        principalSchema: "Organizational",
                        principalTable: "OrganizationalUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrganizationalUnitBranches",
                schema: "Organizational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    OrganizationUnitId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationalUnitBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationalUnitBranches_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Organizational",
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrganizationalUnitBranches_OrganizationalUnits_OrganizationUnitId",
                        column: x => x.OrganizationUnitId,
                        principalSchema: "Organizational",
                        principalTable: "OrganizationalUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccessPoints",
                schema: "Operational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    QrCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccessPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccessPoints_CustomTypes_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "Parameter",
                        principalTable: "CustomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccessPoints_Events_EventId",
                        column: x => x.EventId,
                        principalSchema: "Operational",
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventTargetAudience",
                schema: "Organizational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTargetAudience", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTargetAudience_CustomTypes_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "Parameter",
                        principalTable: "CustomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventTargetAudience_Events_EventId",
                        column: x => x.EventId,
                        principalSchema: "Operational",
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonDivisionProfiles",
                schema: "Organizational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<int>(type: "int", nullable: false),
                    InternalDivisionId = table.Column<int>(type: "int", nullable: false),
                    IsCurrentlySelected = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonDivisionProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonDivisionProfiles_InternalDivisions_InternalDivisionId",
                        column: x => x.InternalDivisionId,
                        principalSchema: "Organizational",
                        principalTable: "InternalDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonDivisionProfiles_People_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "ModelSecurity",
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonDivisionProfiles_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalSchema: "Organizational",
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Attendances",
                schema: "Operational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeOfEntry = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeOfExit = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccessPointOfEntry = table.Column<int>(type: "int", nullable: true),
                    AccessPointOfExit = table.Column<int>(type: "int", nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    QrCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attendances_AccessPoints_AccessPointOfEntry",
                        column: x => x.AccessPointOfEntry,
                        principalSchema: "Operational",
                        principalTable: "AccessPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attendances_AccessPoints_AccessPointOfExit",
                        column: x => x.AccessPointOfExit,
                        principalSchema: "Operational",
                        principalTable: "AccessPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Attendances_People_PersonId",
                        column: x => x.PersonId,
                        principalSchema: "ModelSecurity",
                        principalTable: "People",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                schema: "Organizational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QRCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    PersonDivissionProfileId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_PersonDivisionProfiles_PersonDivissionProfileId",
                        column: x => x.PersonDivissionProfileId,
                        principalSchema: "Organizational",
                        principalTable: "PersonDivisionProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cards_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "Parameter",
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "Areas",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, "Área relacionada con sistemas, informática y desarrollo tecnológico", false, "Tecnología" },
                    { 2, "Área enfocada en estudios sociales, filosofía, literatura y cultura", false, "Humanidades" },
                    { 3, "Área de física, química, biología y otras ciencias naturales", false, "Ciencias" },
                    { 4, "Área dedicada a la enseñanza y formación académica", false, "Educación" },
                    { 5, "Área de gestión institucional y procesos administrativos", false, "Administración" }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "Cities",
                columns: new[] { "Id", "DeparmentId", "DepartmentId", "Name" },
                values: new object[,]
                {
                    { 1, 1, null, "Bogotá" },
                    { 2, 2, null, "Medellín" },
                    { 3, 3, null, "Cali" }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "Departments",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Cundinamarca" },
                    { 2, "Antioquia" },
                    { 3, "Valle del Cauca" }
                });

            migrationBuilder.InsertData(
                schema: "Operational",
                table: "EventTypes",
                columns: new[] { "Id", "Description", "IsDeleted", "Name" },
                values: new object[,]
                {
                    { 1, "Eventos de bienvenida institucional", false, "Bienvenida" },
                    { 2, "Reuniones privadas para planificación interna", false, "Planificación" },
                    { 3, "Sesiones de formación para empleados o estudiantes", false, "Capacitación" },
                    { 4, "Espacios destinados a la concentración y repaso académico", false, "Jornada de Estudio" },
                    { 5, "Actividades laborales organizadas por jornada", false, "Jornada de Trabajo" },
                    { 6, "Eventos prácticos y participativos", false, "Taller" },
                    { 7, "Reuniones de carácter informal o comunitario", false, "Encuentro" }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Modules",
                columns: new[] { "Id", "Description", "Icon", "Name" },
                values: new object[,]
                {
                    { 1, "Grupo principal de navegación", "home", "Menú Principal" },
                    { 2, "Dominio Organizacional", "apartment", "Organizacional" },
                    { 3, "Dominio Operacional", "event_available", "Operacional" },
                    { 4, "Parámetros y configuración", "settings_applications", "Parámetros" },
                    { 5, "Dominio de seguridad", "admin_panel_settings", "Seguridad" }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "OrganizationalUnits",
                columns: new[] { "Id", "BranchId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, null, "Facultad de Ingeniería" },
                    { 2, null, null, "Facultad de Ciencias Económicas" },
                    { 3, null, null, "Facultad de Artes" }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "People",
                columns: new[] { "Id", "Address", "BloodTypeId", "CityId", "DocumentNumber", "DocumentTypeId", "Email", "FirstName", "LastName", "MiddleName", "Phone", "Photo", "SecondLastName" },
                values: new object[,]
                {
                    { 1, null, null, null, "1234567890", null, null, "Demo", "Funcionario", null, "3200001111", null, null },
                    { 2, null, null, null, "9876543210", null, null, "Laura", "Estudiante", null, "3100002222", null, null },
                    { 3, null, null, null, "1122334455", null, null, "Ana", "Administrador", null, "3001234567", null, null },
                    { 4, null, null, null, "9988776655", null, null, "José", "Usuario", null, "3151234567", null, null },
                    { 5, null, null, null, "1234561630", null, null, "María", "Tovar", null, "3200056311", null, null },
                    { 6, null, null, null, "1245567890", null, null, "Camilo", "Charry", null, "3200014311", null, null },
                    { 7, null, null, null, "1235267890", null, null, "Marcos", "Alvarez", null, "320026111", null, null }
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
                schema: "Organizational",
                table: "Profiles",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Perfil para estudiantes de la institución", "Estudiante" },
                    { 2, "Perfil para docentes o instructores", "Profesor" },
                    { 3, "Perfil para personal administrativo", "Administrativo" },
                    { 4, "Perfil para pasantes o practicantes", "Pasante" },
                    { 5, "Perfil para usuarios externos o visitantes", "Invitado" }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Roles",
                columns: new[] { "Id", "Description", "HasAllPermissions", "Name" },
                values: new object[,]
                {
                    { 1, "Acceso total al sistema.", true, "SuperAdmin" },
                    { 2, "Administra carnets y eventos de su organización.", false, "OrgAdmin" },
                    { 3, "Gestiona únicamente los eventos (creación, control y reportes).", false, "Supervisor" },
                    { 4, "Funcionario (docentes, coordinadores, etc.) con visualización de su propio carnet.", false, "Administrativo" },
                    { 5, "Consulta su propio carnet y asistencia.", false, "Estudiante" },
                    { 6, "Acceso mínimo/público.", false, "Usuario" }
                });

            migrationBuilder.InsertData(
                schema: "Parameter",
                table: "Statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Activo" },
                    { 2, "Inactivo" },
                    { 3, "Pendiente" },
                    { 4, "Procesando" },
                    { 5, "Rechazado" },
                    { 6, "Entregado" },
                    { 7, "Leída" },
                    { 8, "En curso" },
                    { 9, "Finalizado" },
                    { 10, "Cancelado" },
                    { 11, "Expirado" },
                    { 12, "Renovado" }
                });

            migrationBuilder.InsertData(
                schema: "Parameter",
                table: "TypeCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Organización" },
                    { 2, "Punto de acceso" },
                    { 3, "Notificación" },
                    { 4, "Tipo de documento" },
                    { 5, "Tipo de sangre" },
                    { 6, "Filtros para eventos privados" }
                });

            migrationBuilder.InsertData(
                schema: "Parameter",
                table: "CustomTypes",
                columns: new[] { "Id", "Description", "Name", "TypeCategoryId" },
                values: new object[,]
                {
                    { 1, "Cédula de ciudadanía", "CC", 4 },
                    { 2, "Cédula de extranjería", "CE", 4 },
                    { 3, "Tarjeta de identidad", "TI", 4 },
                    { 4, "Pasaporte", "PA", 4 },
                    { 5, "Número de Identificación Tributaria", "NIT", 4 },
                    { 6, "Sangre tipo O positivo", "O+", 5 },
                    { 7, "Sangre tipo O negativo", "O-", 5 },
                    { 8, "Sangre tipo A positivo", "A+", 5 },
                    { 9, "Sangre tipo A negativo", "A-", 5 },
                    { 10, "Sangre tipo B positivo", "B+", 5 },
                    { 11, "Sangre tipo B negativo", "B-", 5 },
                    { 12, "Sangre tipo AB positivo", "AB+", 5 },
                    { 13, "Sangre tipo AB negativo", "AB-", 5 },
                    { 14, "Organización tipo empresa", "Empresa", 1 },
                    { 15, "Organización tipo colegio", "Colegio", 1 },
                    { 16, "Organización tipo universidad", "Universidad", 1 },
                    { 17, "Organización sede principal", "Sede Principal", 1 },
                    { 18, "Organización tipo sucursal", "Sucursal", 1 },
                    { 19, "Notificación para verificación de identidad o datos", "Verificación", 3 },
                    { 20, "Notificación de invitación a evento o sistema", "Invitación", 3 },
                    { 21, "Notificación de recordatorio de evento o tarea", "Recordatorio", 3 },
                    { 22, "Notificación de alerta por evento crítico", "Alerta", 3 },
                    { 23, "Punto de acceso solo de entrada", "Entrada", 2 },
                    { 24, "Punto de acceso solo de salida", "Salida", 2 },
                    { 25, "Punto de acceso bidireccional", "Entrada y salida", 2 },
                    { 26, "Descripción", "Division", 6 },
                    { 27, "Descripción", "Profile", 6 },
                    { 28, "Descripción", "Perfil", 6 }
                });

            migrationBuilder.InsertData(
                schema: "Operational",
                table: "Events",
                columns: new[] { "Id", "Code", "Description", "EventEnd", "EventStart", "EventTypeId", "IsPublic", "Name", "ScheduleDate", "ScheduleTime", "SheduleId", "StatusId" },
                values: new object[] { 1, "TECH2025", null, new DateTime(2023, 7, 30, 14, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 7, 30, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, true, "Conferencia de Tecnología", new DateTime(2023, 7, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1900, 1, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), null, 1 });

            migrationBuilder.InsertData(
                schema: "Operational",
                table: "Events",
                columns: new[] { "Id", "Code", "Description", "EventEnd", "EventStart", "EventTypeId", "Name", "ScheduleDate", "ScheduleTime", "SheduleId", "StatusId" },
                values: new object[] { 2, "SALUD2025", null, new DateTime(2023, 8, 5, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 8, 5, 9, 0, 0, 0, DateTimeKind.Unspecified), 2, "Charla de Salud", new DateTime(2023, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1900, 1, 1, 9, 0, 0, 0, DateTimeKind.Unspecified), null, 1 });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Forms",
                columns: new[] { "Id", "Description", "Icon", "ModuleId", "Name", "Url" },
                values: new object[,]
                {
                    { 1, "Panel principal", "home", 1, "Inicio", "/dashboard" },
                    { 2, "Centro de ayuda y documentación", "help", 1, "Ayuda", "/dashboard/ayuda" },
                    { 3, "Vista general de la estructura", "dashboard_customize", 2, "Resumen", "/dashboard/organizational/structure" },
                    { 4, "Administración de sucursales", "store", 2, "Sucursales", "/dashboard/organizational/structure/branch" },
                    { 5, "Gestión de unidades organizativas", "schema", 2, "Unidades Organizativas", "/dashboard/organizational/structure/unit" },
                    { 6, "Administración de divisiones internas", "account_tree", 2, "Divisiones Internas", "/dashboard/organizational/structure/internal-division" },
                    { 7, "Perfiles de las personas en el sistema", "badge", 2, "Perfiles", "/dashboard/organizational/profile" },
                    { 8, "Configuración de horarios/jornadas", "schedule", 2, "Jornadas", "/dashboard/organizational/structure/schedule" },
                    { 9, "Gestión de eventos", "event", 3, "Eventos", "/dashboard/operational/events" },
                    { 10, "Catálogo de tipos de evento", "category", 3, "Tipos de Evento", "/dashboard/operational/event-types" },
                    { 11, "Administración de puntos de acceso", "sensor_door", 3, "Puntos de Acceso", "/dashboard/operational/access-points" },
                    { 12, "Registro y consulta de asistencias", "how_to_reg", 3, "Asistencias", "/dashboard/operational/attendance" },
                    { 13, "Estados del sistema", "check_circle_unread", 4, "Estados", "/dashboard/parametros/status" },
                    { 14, "Tipos y categorías del sistema", "category", 4, "Tipos y Categorías", "/dashboard/parametros/types-category" },
                    { 15, "Configuración del Menú del sistema", "background_dot_small", 4, "Menu", "/dashboard/parametros/menu" },
                    { 16, "Catálogo de departamentos", "flag", 4, "Departamentos", "/dashboard/organizational/location/department" },
                    { 17, "Catálogo de municipios", "place", 4, "Municipios", "/dashboard/organizational/location/municipality" },
                    { 18, "Gestión de personas", "person_pin_circle", 5, "Personas", "/dashboard/seguridad/people" },
                    { 19, "Gestión de usuarios", "groups_2", 5, "Usuarios", "/dashboard/seguridad/users" },
                    { 20, "Gestión de roles", "add_moderator", 5, "Roles", "/dashboard/seguridad/roles" },
                    { 21, "Permisos por formulario", "folder_managed", 5, "Gestión de Permisos", "/dashboard/seguridad/permission-forms" },
                    { 22, "Catálogo de permisos", "lock_open_circle", 5, "Permisos", "/dashboard/seguridad/permissions" },
                    { 23, "Catálogo de formularios", "lists", 5, "Formularios", "/dashboard/seguridad/forms" },
                    { 24, "Catálogo de módulos", "dashboard_2", 5, "Módulos", "/dashboard/seguridad/modules" }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "InternalDivisions",
                columns: new[] { "Id", "AreaCategoryId", "BranchId", "Description", "Name", "OrganizationalUnitId" },
                values: new object[,]
                {
                    { 1, 1, null, "División académica enfocada en ingeniería de software y sistemas.", "Escuela de Sistemas", 1 },
                    { 2, 1, null, "División académica centrada en ingeniería civil y estructuras.", "Escuela de Civil", 1 },
                    { 3, 4, null, "Encargado de contabilidad, auditoría y normativas contables.", "Departamento de Contaduría", 2 },
                    { 4, 4, null, "Área enfocada en teoría económica, micro y macroeconomía.", "Departamento de Economía", 2 },
                    { 5, 2, null, "Formación profesional en teoría musical, instrumentos y composición.", "Escuela de Música", 3 }
                });

            migrationBuilder.InsertData(
                schema: "Parameter",
                table: "MenuStructures",
                columns: new[] { "Id", "FormId", "Icon", "IsDeleted", "ModuleId", "OrderIndex", "ParentMenuId", "Title", "Type" },
                values: new object[,]
                {
                    { 1, null, null, false, 1, 1, null, null, "group" },
                    { 2, null, null, false, 2, 2, null, null, "group" },
                    { 3, null, null, false, 3, 3, null, null, "group" },
                    { 4, null, null, false, 4, 4, null, null, "group" },
                    { 5, null, null, false, 5, 5, null, null, "group" }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Users",
                columns: new[] { "Id", "Active", "DateCreated", "IsDeleted", "Password", "PersonId", "RefreshToken", "RefreshTokenExpiryTime", "ResetCode", "ResetCodeExpiration", "TempCodeAttempts", "TempCodeConsumedAt", "TempCodeCreatedAt", "TempCodeExpiresAt", "TempCodeHash", "TempCodeResendBlockedUntil", "UserName" },
                values: new object[,]
                {
                    { 1, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "123", 1, null, null, null, null, 0, null, null, null, null, null, "admin" },
                    { 2, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Marcos2025", 7, null, null, null, null, 0, null, null, null, null, null, "marcosrojasalvarez09172007@gmail.com" },
                    { 3, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "isa123", 5, null, null, null, null, 0, null, null, null, null, null, "isabeltovarp.18@gmail.com" },
                    { 4, true, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Katalin@01", 6, null, null, null, null, 0, null, null, null, null, null, "cachape64@gmail.com" }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Users",
                columns: new[] { "Id", "DateCreated", "IsDeleted", "Password", "PersonId", "RefreshToken", "RefreshTokenExpiryTime", "ResetCode", "ResetCodeExpiration", "TempCodeAttempts", "TempCodeConsumedAt", "TempCodeCreatedAt", "TempCodeExpiresAt", "TempCodeHash", "TempCodeResendBlockedUntil", "UserName" },
                values: new object[,]
                {
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "L4d!Estudiante2025", 2, null, null, null, null, 0, null, null, null, null, null, null },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Adm!nCarnet2025", 3, null, null, null, null, 0, null, null, null, null, null, null },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Usr!Carnet2025", 4, null, null, null, null, 0, null, null, null, null, null, null }
                });

            migrationBuilder.InsertData(
                schema: "Operational",
                table: "AccessPoints",
                columns: new[] { "Id", "Description", "EventId", "Name", "QrCode", "TypeId" },
                values: new object[,]
                {
                    { 1, "Acceso norte del evento", 1, "Punto Norte", null, 1 },
                    { 2, "Acceso sur del evento", 1, "Punto Sur", null, 2 },
                    { 3, "Acceso principal", 2, "Punto Principal", null, 1 }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "EventTargetAudience",
                columns: new[] { "Id", "EventId", "IsDeleted", "ReferenceId", "TypeId" },
                values: new object[,]
                {
                    { 1, 1, false, 1, 6 },
                    { 2, 1, false, 2, 6 },
                    { 3, 2, false, 3, 6 }
                });

            migrationBuilder.InsertData(
                schema: "Parameter",
                table: "MenuStructures",
                columns: new[] { "Id", "FormId", "Icon", "IsDeleted", "ModuleId", "OrderIndex", "ParentMenuId", "Title", "Type" },
                values: new object[,]
                {
                    { 6, 1, null, false, null, 1, 1, null, "item" },
                    { 7, 2, null, false, null, 2, 1, null, "item" },
                    { 8, null, "account_tree", false, null, 1, 2, "Estructura Organizativa", "collapse" },
                    { 15, null, "event_available", false, null, 1, 3, "Eventos y Control de Acceso", "collapse" },
                    { 20, null, "settings_applications", false, null, 1, 4, "Configuración General", "collapse" },
                    { 21, null, "location_on", false, null, 2, 4, "Ubicación", "collapse" },
                    { 27, null, "admin_panel_settings", false, null, 1, 5, "Gestión de Seguridad", "collapse" }
                });

            migrationBuilder.InsertData(
                schema: "Notifications",
                table: "Notification",
                columns: new[] { "Id", "CreateDate", "IsDeleted", "Message", "NotificationTypeId", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 7, 27, 10, 0, 0, 0, DateTimeKind.Unspecified), false, "Por favor verifica tu cuenta haciendo clic en el enlace enviado.", 1, "Verificación de cuenta" },
                    { 2, new DateTime(2025, 7, 28, 9, 30, 0, 0, DateTimeKind.Unspecified), false, "Estás invitado al evento de bienvenida. Confirma tu asistencia.", 2, "Invitación a evento" }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "Organizations",
                columns: new[] { "Id", "Description", "Logo", "Name", "TypeId" },
                values: new object[,]
                {
                    { 1, "Institución de educación superior", "logo_unal.png", "Universidad Nacional", 1 },
                    { 2, "Centro de atención médica", "logo_hsj.png", "Hospital San José", 2 },
                    { 3, "Fundación sin ánimo de lucro", "logo_fundacion.png", "Fundación Futuro", 3 }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "PersonDivisionProfiles",
                columns: new[] { "Id", "InternalDivisionId", "PersonId", "ProfileId" },
                values: new object[] { 1, 1, 1, 1 });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "PersonDivisionProfiles",
                columns: new[] { "Id", "InternalDivisionId", "IsCurrentlySelected", "PersonId", "ProfileId" },
                values: new object[] { 2, 1, true, 2, 2 });

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
                table: "UserRoles",
                columns: new[] { "Id", "IsDeleted", "RolId", "UserId" },
                values: new object[,]
                {
                    { 1, false, 1, 1 },
                    { 2, false, 2, 2 },
                    { 3, false, 3, 3 },
                    { 4, false, 4, 4 },
                    { 5, false, 1, 2 },
                    { 6, false, 1, 3 },
                    { 7, false, 1, 4 }
                });

            migrationBuilder.InsertData(
                schema: "Operational",
                table: "Attendances",
                columns: new[] { "Id", "AccessPointOfEntry", "AccessPointOfExit", "PersonId", "QrCode", "TimeOfEntry", "TimeOfExit" },
                values: new object[,]
                {
                    { 1, 1, 2, 1, null, new DateTime(2023, 1, 1, 8, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, 2, 2, null, new DateTime(2023, 1, 1, 9, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 1, 1, 13, 45, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "Branches",
                columns: new[] { "Id", "Address", "CityId", "Email", "Location", "Name", "OrganizationId", "Phone" },
                values: new object[,]
                {
                    { 1, "Calle 1 # 2-34", 1, "principal@org.com", "Centro", "Sucursal Principal", 1, "123456789" },
                    { 2, "Carrera 45 # 67-89", 1, "norte@org.com", "Zona Norte", "Sucursal Norte", 1, "987654321" }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "Cards",
                columns: new[] { "Id", "CreationDate", "ExpirationDate", "IsDeleted", "PersonDivissionProfileId", "QRCode", "StatusId" },
                values: new object[] { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1, "QR12345", 1 });

            migrationBuilder.InsertData(
                schema: "Parameter",
                table: "MenuStructures",
                columns: new[] { "Id", "FormId", "Icon", "IsDeleted", "ModuleId", "OrderIndex", "ParentMenuId", "Title", "Type" },
                values: new object[,]
                {
                    { 9, 3, null, false, null, 1, 8, null, "item" },
                    { 10, 4, null, false, null, 2, 8, null, "item" },
                    { 11, 5, null, false, null, 3, 8, null, "item" },
                    { 12, 6, null, false, null, 4, 8, null, "item" },
                    { 13, 7, null, false, null, 5, 8, null, "item" },
                    { 14, 8, null, false, null, 6, 8, null, "item" },
                    { 16, 9, null, false, null, 1, 15, null, "item" },
                    { 17, 10, null, false, null, 2, 15, null, "item" },
                    { 18, 11, null, false, null, 3, 15, null, "item" },
                    { 19, 12, null, false, null, 4, 15, null, "item" },
                    { 22, 13, null, false, null, 1, 20, null, "item" },
                    { 23, 14, null, false, null, 2, 20, null, "item" },
                    { 24, 15, null, false, null, 3, 20, null, "item" },
                    { 25, 16, null, false, null, 1, 21, null, "item" },
                    { 26, 17, null, false, null, 2, 21, null, "item" },
                    { 28, 18, null, false, null, 1, 27, null, "item" },
                    { 29, 19, null, false, null, 2, 27, null, "item" },
                    { 30, 20, null, false, null, 3, 27, null, "item" },
                    { 31, 21, null, false, null, 4, 27, null, "item" },
                    { 32, 22, null, false, null, 5, 27, null, "item" },
                    { 33, 23, null, false, null, 6, 27, null, "item" },
                    { 34, 24, null, false, null, 7, 27, null, "item" }
                });

            migrationBuilder.InsertData(
                schema: "Notifications",
                table: "NotificationReceived",
                columns: new[] { "Id", "ExpirationDate", "IsDeleted", "NotificationId", "ReadDate", "SendDate", "StatusId", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 1, null, new DateTime(2025, 7, 27, 10, 5, 0, 0, DateTimeKind.Unspecified), 1, 1 },
                    { 2, new DateTime(2025, 8, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), false, 2, new DateTime(2025, 7, 28, 10, 15, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 28, 9, 35, 0, 0, DateTimeKind.Unspecified), 2, 2 }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "Schedules",
                columns: new[] { "Id", "EndTime", "Name", "OrganizationId", "StartTime" },
                values: new object[,]
                {
                    { 1, new TimeSpan(0, 18, 0, 0, 0), "Horario Jornada A", 1, new TimeSpan(0, 7, 0, 0, 0) },
                    { 2, new TimeSpan(0, 17, 0, 0, 0), "Horario Jornada B", 1, new TimeSpan(0, 8, 0, 0, 0) },
                    { 3, new TimeSpan(0, 19, 0, 0, 0), "Horario Jornada C", 1, new TimeSpan(0, 6, 30, 0, 0) }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "OrganizationalUnitBranches",
                columns: new[] { "Id", "BranchId", "OrganizationUnitId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 1 },
                    { 3, 1, 2 },
                    { 4, 2, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccessPoints_EventId",
                schema: "Operational",
                table: "AccessPoints",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_AccessPoints_TypeId",
                schema: "Operational",
                table: "AccessPoints",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_AccessPointOfEntry",
                schema: "Operational",
                table: "Attendances",
                column: "AccessPointOfEntry");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_AccessPointOfExit",
                schema: "Operational",
                table: "Attendances",
                column: "AccessPointOfExit");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_PersonId",
                schema: "Operational",
                table: "Attendances",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CityId",
                schema: "Organizational",
                table: "Branches",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Name",
                schema: "Organizational",
                table: "Branches",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_OrganizationId",
                schema: "Organizational",
                table: "Branches",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_PersonDivissionProfileId",
                schema: "Organizational",
                table: "Cards",
                column: "PersonDivissionProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cards_StatusId",
                schema: "Organizational",
                table: "Cards",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_DepartmentId",
                schema: "Organizational",
                table: "Cities",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_Name",
                schema: "Organizational",
                table: "Cities",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomTypes_Name",
                schema: "Parameter",
                table: "CustomTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomTypes_TypeCategoryId",
                schema: "Parameter",
                table: "CustomTypes",
                column: "TypeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Name",
                schema: "Organizational",
                table: "Departments",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventTypeId",
                schema: "Operational",
                table: "Events",
                column: "EventTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_SheduleId",
                schema: "Operational",
                table: "Events",
                column: "SheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_StatusId",
                schema: "Operational",
                table: "Events",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTargetAudience_EventId",
                schema: "Organizational",
                table: "EventTargetAudience",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTargetAudience_TypeId",
                schema: "Organizational",
                table: "EventTargetAudience",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_ModuleId",
                schema: "ModelSecurity",
                table: "Forms",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Forms_Name",
                schema: "ModelSecurity",
                table: "Forms",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Forms_Url",
                schema: "ModelSecurity",
                table: "Forms",
                column: "Url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternalDivisions_AreaCategoryId",
                schema: "Organizational",
                table: "InternalDivisions",
                column: "AreaCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalDivisions_BranchId",
                schema: "Organizational",
                table: "InternalDivisions",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_InternalDivisions_Name",
                schema: "Organizational",
                table: "InternalDivisions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternalDivisions_OrganizationalUnitId",
                schema: "Organizational",
                table: "InternalDivisions",
                column: "OrganizationalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuStructures_FormId",
                schema: "Parameter",
                table: "MenuStructures",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuStructures_ModuleId",
                schema: "Parameter",
                table: "MenuStructures",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuStructures_ParentMenuId_OrderIndex",
                schema: "Parameter",
                table: "MenuStructures",
                columns: new[] { "ParentMenuId", "OrderIndex" });

            migrationBuilder.CreateIndex(
                name: "IX_Modules_Name",
                schema: "ModelSecurity",
                table: "Modules",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationTypeId",
                schema: "Notifications",
                table: "Notification",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationReceived_NotificationId",
                schema: "Notifications",
                table: "NotificationReceived",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationReceived_StatusId",
                schema: "Notifications",
                table: "NotificationReceived",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationReceived_UserId",
                schema: "Notifications",
                table: "NotificationReceived",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnitBranches_BranchId",
                schema: "Organizational",
                table: "OrganizationalUnitBranches",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnitBranches_OrganizationUnitId",
                schema: "Organizational",
                table: "OrganizationalUnitBranches",
                column: "OrganizationUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnits_BranchId",
                schema: "Organizational",
                table: "OrganizationalUnits",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationalUnits_Name",
                schema: "Organizational",
                table: "OrganizationalUnits",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Name",
                schema: "Organizational",
                table: "Organizations",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_TypeId",
                schema: "Organizational",
                table: "Organizations",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_People_BloodTypeId",
                schema: "ModelSecurity",
                table: "People",
                column: "BloodTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_People_CityId",
                schema: "ModelSecurity",
                table: "People",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_People_DocumentNumber",
                schema: "ModelSecurity",
                table: "People",
                column: "DocumentNumber",
                unique: true,
                filter: "[DocumentNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_People_DocumentTypeId",
                schema: "ModelSecurity",
                table: "People",
                column: "DocumentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_Name",
                schema: "ModelSecurity",
                table: "Permissions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonDivisionProfiles_InternalDivisionId",
                schema: "Organizational",
                table: "PersonDivisionProfiles",
                column: "InternalDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonDivisionProfiles_PersonId",
                schema: "Organizational",
                table: "PersonDivisionProfiles",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonDivisionProfiles_PersonId_ProfileId_InternalDivisionId",
                schema: "Organizational",
                table: "PersonDivisionProfiles",
                columns: new[] { "PersonId", "ProfileId", "InternalDivisionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonDivisionProfiles_ProfileId",
                schema: "Organizational",
                table: "PersonDivisionProfiles",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_Name",
                schema: "Organizational",
                table: "Profiles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_TokenHash",
                schema: "Auth",
                table: "RefreshToken",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                schema: "ModelSecurity",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolFormPermissions_FormId",
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                column: "FormId");

            migrationBuilder.CreateIndex(
                name: "IX_RolFormPermissions_PermissionId",
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolFormPermissions_RolId_FormId_PermissionId",
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                columns: new[] { "RolId", "FormId", "PermissionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedules_OrganizationId",
                schema: "Organizational",
                table: "Schedules",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Statuses_Name",
                schema: "Parameter",
                table: "Statuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TypeCategories_Name",
                schema: "Parameter",
                table: "TypeCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RolId_UserId",
                schema: "ModelSecurity",
                table: "UserRoles",
                columns: new[] { "RolId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                schema: "ModelSecurity",
                table: "UserRoles",
                column: "UserId");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendances",
                schema: "Operational");

            migrationBuilder.DropTable(
                name: "Cards",
                schema: "Organizational");

            migrationBuilder.DropTable(
                name: "EventTargetAudience",
                schema: "Organizational");

            migrationBuilder.DropTable(
                name: "MenuStructures",
                schema: "Parameter");

            migrationBuilder.DropTable(
                name: "NotificationReceived",
                schema: "Notifications");

            migrationBuilder.DropTable(
                name: "OrganizationalUnitBranches",
                schema: "Organizational");

            migrationBuilder.DropTable(
                name: "RefreshToken",
                schema: "Auth");

            migrationBuilder.DropTable(
                name: "RolFormPermissions",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "UserRoles",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "AccessPoints",
                schema: "Operational");

            migrationBuilder.DropTable(
                name: "PersonDivisionProfiles",
                schema: "Organizational");

            migrationBuilder.DropTable(
                name: "Notification",
                schema: "Notifications");

            migrationBuilder.DropTable(
                name: "Forms",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "Permissions",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "Events",
                schema: "Operational");

            migrationBuilder.DropTable(
                name: "InternalDivisions",
                schema: "Organizational");

            migrationBuilder.DropTable(
                name: "Profiles",
                schema: "Organizational");

            migrationBuilder.DropTable(
                name: "Modules",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "People",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "EventTypes",
                schema: "Operational");

            migrationBuilder.DropTable(
                name: "Schedules",
                schema: "Organizational");

            migrationBuilder.DropTable(
                name: "Statuses",
                schema: "Parameter");

            migrationBuilder.DropTable(
                name: "Areas",
                schema: "Organizational");

            migrationBuilder.DropTable(
                name: "OrganizationalUnits",
                schema: "Organizational");

            migrationBuilder.DropTable(
                name: "Branches",
                schema: "Organizational");

            migrationBuilder.DropTable(
                name: "Cities",
                schema: "Organizational");

            migrationBuilder.DropTable(
                name: "Organizations",
                schema: "Organizational");

            migrationBuilder.DropTable(
                name: "Departments",
                schema: "Organizational");

            migrationBuilder.DropTable(
                name: "CustomTypes",
                schema: "Parameter");

            migrationBuilder.DropTable(
                name: "TypeCategories",
                schema: "Parameter");
        }
    }
}
