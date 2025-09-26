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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CardTemplates",
                schema: "Operational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FrontBackgroundUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackBackgroundUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FrontElementsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BackElementsJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                schema: "Organizational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportBatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalRows = table.Column<int>(type: "int", nullable: false),
                    SuccessCount = table.Column<int>(type: "int", nullable: false),
                    ErrorCount = table.Column<int>(type: "int", nullable: false),
                    ContextJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportBatches", x => x.Id);
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "AccessPoints",
                schema: "Operational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    QrCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    CustomTypeId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notification_CustomTypes_CustomTypeId",
                        column: x => x.CustomTypeId,
                        principalSchema: "Parameter",
                        principalTable: "CustomTypes",
                        principalColumn: "Id");
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        principalColumn: "Id");
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                        onDelete: ReferentialAction.Cascade);
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "EventAccessPoints",
                schema: "Operational",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false),
                    AccessPointId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAccessPoints", x => new { x.EventId, x.AccessPointId });
                    table.ForeignKey(
                        name: "FK_EventAccessPoints_AccessPoints_AccessPointId",
                        column: x => x.AccessPointId,
                        principalSchema: "Operational",
                        principalTable: "AccessPoints",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventAccessPoints_Events_EventId",
                        column: x => x.EventId,
                        principalSchema: "Operational",
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventTargetAudiences",
                schema: "Operational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    ReferenceId = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<int>(type: "int", nullable: true),
                    OrganizationalUnitId = table.Column<int>(type: "int", nullable: true),
                    InternalDivisionId = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventTargetAudiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventTargetAudiences_CustomTypes_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "Parameter",
                        principalTable: "CustomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventTargetAudiences_Events_EventId",
                        column: x => x.EventId,
                        principalSchema: "Operational",
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventTargetAudiences_InternalDivisions_InternalDivisionId",
                        column: x => x.InternalDivisionId,
                        principalSchema: "Organizational",
                        principalTable: "InternalDivisions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventTargetAudiences_OrganizationalUnits_OrganizationalUnitId",
                        column: x => x.OrganizationalUnitId,
                        principalSchema: "Organizational",
                        principalTable: "OrganizationalUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventTargetAudiences_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalSchema: "Organizational",
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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
                name: "Cards",
                schema: "Organizational",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QRCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UniqueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CardTemplateId = table.Column<int>(type: "int", nullable: false),
                    PersonDivissionProfileId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cards_CardTemplates_CardTemplateId",
                        column: x => x.CardTemplateId,
                        principalSchema: "Operational",
                        principalTable: "CardTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "ImportBatchRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportBatchId = table.Column<int>(type: "int", nullable: false),
                    RowNumber = table.Column<int>(type: "int", nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    PersonDivisionProfileId = table.Column<int>(type: "int", nullable: true),
                    CardId = table.Column<int>(type: "int", nullable: true),
                    UpdatedPhoto = table.Column<bool>(type: "bit", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportBatchRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportBatchRows_Cards_CardId",
                        column: x => x.CardId,
                        principalSchema: "Organizational",
                        principalTable: "Cards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ImportBatchRows_ImportBatches_ImportBatchId",
                        column: x => x.ImportBatchId,
                        principalTable: "ImportBatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ImportBatchRows_PersonDivisionProfiles_PersonDivisionProfileId",
                        column: x => x.PersonDivisionProfileId,
                        principalSchema: "Organizational",
                        principalTable: "PersonDivisionProfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "Areas",
                columns: new[] { "Id", "Code", "CreateAt", "Description", "IsDeleted", "Name", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Área relacionada con sistemas, informática y desarrollo tecnológico", false, "Tecnología", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Área enfocada en estudios sociales, filosofía, literatura y cultura", false, "Humanidades", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Área de física, química, biología y otras ciencias naturales", false, "Ciencias", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Área dedicada a la enseñanza y formación académica", false, "Educación", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Área de gestión institucional y procesos administrativos", false, "Administración", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Operational",
                table: "CardTemplates",
                columns: new[] { "Id", "BackBackgroundUrl", "BackElementsJson", "Code", "CreateAt", "FrontBackgroundUrl", "FrontElementsJson", "Name", "UpdateAt" },
                values: new object[] { 1, "https://drgxicjtijjdhrvsjgvd.supabase.co/storage/v1/object/public/Templates/ladotrasero.svg", "\r\n                    {\r\n                        \"title\": { \"x\": 91, \"y\": 202 },\r\n                        \"guides\": { \"x\": 36, \"y\": 371 },\r\n                        \"address\": { \"x\": 43, \"y\": 568 },\r\n                        \"phoneNumber\": { \"x\": 269, \"y\": 568 },\r\n                        \"email\": { \"x\": 271, \"y\": 590 }\r\n                    }", null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "https://drgxicjtijjdhrvsjgvd.supabase.co/storage/v1/object/public/Templates/ladoprincipal.svg", "\r\n                    {\r\n                        \"qr\": { \"x\": 332, \"y\": 48 },\r\n                        \"underQrText\": { \"x\": 302, \"y\": 115 },\r\n                        \"companyName\": { \"x\": 70, \"y\": 78 },\r\n                        \"logo\": { \"x\": 7, \"y\": 97 },\r\n                        \"userPhoto\": { \"x\": -16, \"y\": -1 },\r\n                        \"name\": { \"x\": 240, \"y\": 209 },\r\n                        \"profile\": { \"x\": 240, \"y\": 333 },\r\n                        \"categoryArea\": { \"x\": 138, \"y\": 371 },\r\n                        \"phoneNumber\": { \"x\": 46, \"y\": 502 },\r\n                        \"bloodTypeValue\": { \"x\": 379, \"y\": 462 },\r\n                        \"email\": { \"x\": 144, \"y\": 560 },\r\n                        \"cardId\": { \"x\": 164, \"y\": 603 }\r\n                    }", "Plantilla Básica", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "Cities",
                columns: new[] { "Id", "Code", "CreateAt", "DeparmentId", "DepartmentId", "Name", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 1, null, "Bogotá", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 2, null, "Medellín", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 3, null, "Cali", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "Departments",
                columns: new[] { "Id", "Code", "CreateAt", "Name", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Cundinamarca", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Antioquia", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Valle del Cauca", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Operational",
                table: "EventTypes",
                columns: new[] { "Id", "Code", "CreateAt", "Description", "IsDeleted", "Name", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Eventos de bienvenida institucional", false, "Bienvenida", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Reuniones privadas para planificación interna", false, "Planificación", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Sesiones de formación para empleados o estudiantes", false, "Capacitación", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Espacios destinados a la concentración y repaso académico", false, "Jornada de Estudio", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Actividades laborales organizadas por jornada", false, "Jornada de Trabajo", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Eventos prácticos y participativos", false, "Taller", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Reuniones de carácter informal o comunitario", false, "Encuentro", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Modules",
                columns: new[] { "Id", "Code", "CreateAt", "Description", "Icon", "Name", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Grupo principal de navegación", "home", "Menú Principal", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Dominio Organizacional", "apartment", "Organizacional", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Dominio Operacional", "event_available", "Operacional", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Parámetros y configuración", "settings_applications", "Parámetros", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Dominio de seguridad", "admin_panel_settings", "Seguridad", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Notifications",
                table: "Notification",
                columns: new[] { "Id", "Code", "CreateAt", "CreateDate", "CustomTypeId", "IsDeleted", "Message", "NotificationTypeId", "Title", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 27, 15, 0, 0, 0, DateTimeKind.Utc), null, false, "Por favor verifica tu cuenta haciendo clic en el enlace enviado.", 1, "Verificación de cuenta", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 28, 14, 30, 0, 0, DateTimeKind.Utc), null, false, "Estás invitado al evento de bienvenida. Confirma tu asistencia.", 2, "Invitación a evento", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "OrganizationalUnits",
                columns: new[] { "Id", "BranchId", "Code", "CreateAt", "Description", "Name", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null, "Facultad de Ingeniería", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null, "Facultad de Ciencias Económicas", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null, "Facultad de Artes", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "People",
                columns: new[] { "Id", "Address", "BloodTypeId", "CityId", "Code", "CreateAt", "DocumentNumber", "DocumentTypeId", "Email", "FirstName", "LastName", "MiddleName", "Phone", "PhotoPath", "PhotoUrl", "SecondLastName", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "1234567890", null, null, "Demo", "Funcionario", null, "3200001111", null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "9876543210", null, null, "Laura", "Estudiante", null, "3100002222", null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "1122334455", null, null, "Ana", "Administrador", null, "3001234567", null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, null, null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "9988776655", null, null, "José", "Usuario", null, "3151234567", null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, null, null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "1234561630", null, null, "María", "Tovar", null, "3200056311", null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, null, null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "1245567890", null, null, "Camilo", "Charry", null, "3200014311", null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, null, null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "1235267890", null, null, "Marcos", "Alvarez", null, "320026111", null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Permissions",
                columns: new[] { "Id", "Code", "CreateAt", "Description", "Name", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Puede crear nuevos registros", "crear", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Puede editar registros existentes", "editar", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Puede validar datos (correo, QR)", "validar", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "Profiles",
                columns: new[] { "Id", "Code", "CreateAt", "Description", "Name", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Perfil para estudiantes de la institución", "Estudiante", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Perfil para docentes o instructores", "Profesor", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Perfil para personal administrativo", "Administrativo", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Perfil para pasantes o practicantes", "Pasante", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Perfil para usuarios externos o visitantes", "Invitado", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Roles",
                columns: new[] { "Id", "Code", "CreateAt", "Description", "HasAllPermissions", "Name", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Acceso total al sistema.", true, "SuperAdmin", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Administra carnets y eventos de su organización.", false, "OrgAdmin", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Gestiona únicamente los eventos (creación, control y reportes).", false, "Supervisor", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Funcionario (docentes, coordinadores, etc.) con visualización de su propio carnet.", false, "Administrativo", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Consulta su propio carnet y asistencia.", false, "Estudiante", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Acceso mínimo/público.", false, "Usuario", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Parameter",
                table: "Statuses",
                columns: new[] { "Id", "Code", "CreateAt", "Name", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Activo", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Inactivo", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Pendiente", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Procesando", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Rechazado", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Entregado", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Leída", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "En curso", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Finalizado", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Cancelado", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Expirado", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Renovado", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Parameter",
                table: "TypeCategories",
                columns: new[] { "Id", "Code", "CreateAt", "Name", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Organización", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Punto de acceso", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Notificación", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Tipo de documento", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Tipo de sangre", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Filtros para eventos privados", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Parameter",
                table: "CustomTypes",
                columns: new[] { "Id", "Code", "CreateAt", "Description", "Name", "TypeCategoryId", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Cédula de ciudadanía", "CC", 4, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Cédula de extranjería", "CE", 4, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Tarjeta de identidad", "TI", 4, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Pasaporte", "PA", 4, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Número de Identificación Tributaria", "NIT", 4, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Sangre tipo O positivo", "O+", 5, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Sangre tipo O negativo", "O-", 5, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Sangre tipo A positivo", "A+", 5, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Sangre tipo A negativo", "A-", 5, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Sangre tipo B positivo", "B+", 5, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Sangre tipo B negativo", "B-", 5, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Sangre tipo AB positivo", "AB+", 5, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Sangre tipo AB negativo", "AB-", 5, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 14, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Organización tipo empresa", "Empresa", 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Organización tipo colegio", "Colegio", 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 16, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Organización tipo universidad", "Universidad", 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 17, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Organización sede principal", "Sede Principal", 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 18, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Organización tipo sucursal", "Sucursal", 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 19, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Notificación para verificación de identidad o datos", "Verificación", 3, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 20, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Notificación de invitación a evento o sistema", "Invitación", 3, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 21, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Notificación de recordatorio de evento o tarea", "Recordatorio", 3, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 22, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Notificación de alerta por evento crítico", "Alerta", 3, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 23, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Punto de acceso solo de entrada", "Entrada", 2, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 24, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Punto de acceso solo de salida", "Salida", 2, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 25, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Punto de acceso bidireccional", "Entrada y salida", 2, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 26, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Descripción", "Division", 6, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 27, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Descripción", "Profile", 6, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 28, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Descripción", "Perfil", 6, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Operational",
                table: "Events",
                columns: new[] { "Id", "Code", "CreateAt", "Description", "EventEnd", "EventStart", "EventTypeId", "IsPublic", "Name", "ScheduleDate", "ScheduleTime", "SheduleId", "StatusId", "UpdateAt" },
                values: new object[] { 1, "TECH2025", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2023, 7, 30, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 7, 30, 15, 0, 0, 0, DateTimeKind.Utc), 1, true, "Conferencia de Tecnología", new DateTime(2023, 7, 30, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1900, 1, 1, 15, 0, 0, 0, DateTimeKind.Utc), null, 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                schema: "Operational",
                table: "Events",
                columns: new[] { "Id", "Code", "CreateAt", "Description", "EventEnd", "EventStart", "EventTypeId", "Name", "ScheduleDate", "ScheduleTime", "SheduleId", "StatusId", "UpdateAt" },
                values: new object[] { 2, "SALUD2025", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2023, 8, 5, 17, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 8, 5, 14, 0, 0, 0, DateTimeKind.Utc), 2, "Charla de Salud", new DateTime(2023, 8, 5, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1900, 1, 1, 14, 0, 0, 0, DateTimeKind.Utc), null, 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Forms",
                columns: new[] { "Id", "Code", "CreateAt", "Description", "Icon", "ModuleId", "Name", "UpdateAt", "Url" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Panel principal", "home", 1, "Inicio", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard" },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Centro de ayuda y documentación", "help", 1, "Ayuda", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/ayuda" },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Vista general de la estructura", "dashboard_customize", 2, "Resumen", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/organizational/structure" },
                    { 4, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Administración de sucursales", "store", 2, "Sucursales", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/organizational/structure/branch" },
                    { 5, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Gestión de unidades organizativas", "schema", 2, "Unidades Organizativas", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/organizational/structure/unit" },
                    { 6, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Administración de divisiones internas", "account_tree", 2, "Divisiones Internas", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/organizational/structure/internal-division" },
                    { 7, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Perfiles de las personas en el sistema", "badge", 2, "Perfiles", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/organizational/profile" },
                    { 8, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Configuración de horarios/jornadas", "schedule", 2, "Jornadas", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/organizational/structure/schedule" },
                    { 9, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Gestión de eventos", "event", 3, "Eventos", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/operational/events" },
                    { 10, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Catálogo de tipos de evento", "category", 3, "Tipos de Evento", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/operational/event-types" },
                    { 11, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Administración de puntos de acceso", "sensor_door", 3, "Puntos de Acceso", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/operational/access-points" },
                    { 12, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Registro y consulta de asistencias", "how_to_reg", 3, "Asistencias", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/operational/attendance" },
                    { 13, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Registro y consulta de generación masiva de carnets", "badge", 3, "Emisión de Carnet", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/operational/card-issuance" },
                    { 14, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Estados del sistema", "check_circle_unread", 4, "Estados", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/parametros/status" },
                    { 15, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Tipos y categorías del sistema", "category", 4, "Tipos y Categorías", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/parametros/types-category" },
                    { 16, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Configuración del Menú del sistema", "background_dot_small", 4, "Menu", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/parametros/menu" },
                    { 17, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Catálogo de departamentos", "flag", 4, "Departamentos", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/organizational/location/department" },
                    { 18, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Catálogo de municipios", "place", 4, "Municipios", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/organizational/location/municipality" },
                    { 19, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Gestión de personas", "person_pin_circle", 5, "Personas", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/seguridad/people" },
                    { 20, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Gestión de usuarios", "groups_2", 5, "Usuarios", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/seguridad/users" },
                    { 21, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Gestión de roles", "add_moderator", 5, "Roles", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/seguridad/roles" },
                    { 22, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Permisos por formulario", "folder_managed", 5, "Gestión de Permisos", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/seguridad/permission-forms" },
                    { 23, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Catálogo de permisos", "lock_open_circle", 5, "Permisos", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/seguridad/permissions" },
                    { 24, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Catálogo de formularios", "lists", 5, "Formularios", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/seguridad/forms" },
                    { 25, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Catálogo de módulos", "dashboard_2", 5, "Módulos", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "/dashboard/seguridad/modules" }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "InternalDivisions",
                columns: new[] { "Id", "AreaCategoryId", "BranchId", "Code", "CreateAt", "Description", "Name", "OrganizationalUnitId", "UpdateAt" },
                values: new object[,]
                {
                    { 1, 1, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "División académica enfocada en ingeniería de software y sistemas.", "Escuela de Sistemas", 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 1, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "División académica centrada en ingeniería civil y estructuras.", "Escuela de Civil", 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 4, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Encargado de contabilidad, auditoría y normativas contables.", "Departamento de Contaduría", 2, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 4, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Área enfocada en teoría económica, micro y macroeconomía.", "Departamento de Economía", 2, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 2, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Formación profesional en teoría musical, instrumentos y composición.", "Escuela de Música", 3, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Parameter",
                table: "MenuStructures",
                columns: new[] { "Id", "Code", "CreateAt", "FormId", "Icon", "IsDeleted", "ModuleId", "OrderIndex", "ParentMenuId", "Title", "Type", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null, null, false, 1, 1, null, null, "group", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null, null, false, 2, 2, null, null, "group", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null, null, false, 3, 3, null, null, "group", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null, null, false, 4, 4, null, null, "group", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null, null, false, 5, 5, null, null, "group", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Users",
                columns: new[] { "Id", "Active", "Code", "CreateAt", "DateCreated", "IsDeleted", "Password", "PersonId", "RefreshToken", "RefreshTokenExpiryTime", "ResetCode", "ResetCodeExpiration", "TempCodeAttempts", "TempCodeConsumedAt", "TempCodeCreatedAt", "TempCodeExpiresAt", "TempCodeHash", "TempCodeResendBlockedUntil", "UpdateAt", "UserName" },
                values: new object[,]
                {
                    { 1, true, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), false, "123", 1, null, null, null, null, 0, null, null, null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "admin" },
                    { 2, true, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), false, "Marcos2025", 7, null, null, null, null, 0, null, null, null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "marcosrojasalvarez09172007@gmail.com" },
                    { 3, true, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), false, "isa123", 5, null, null, null, null, 0, null, null, null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "isabeltovarp.18@gmail.com" },
                    { 4, true, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), false, "Katalin@01", 6, null, null, null, null, 0, null, null, null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "cachape64@gmail.com" }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "Users",
                columns: new[] { "Id", "Code", "CreateAt", "DateCreated", "IsDeleted", "Password", "PersonId", "RefreshToken", "RefreshTokenExpiryTime", "ResetCode", "ResetCodeExpiration", "TempCodeAttempts", "TempCodeConsumedAt", "TempCodeCreatedAt", "TempCodeExpiresAt", "TempCodeHash", "TempCodeResendBlockedUntil", "UpdateAt", "UserName" },
                values: new object[,]
                {
                    { 5, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), false, "L4d!Estudiante2025", 2, null, null, null, null, 0, null, null, null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null },
                    { 6, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), false, "Adm!nCarnet2025", 3, null, null, null, null, 0, null, null, null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null },
                    { 7, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), false, "Usr!Carnet2025", 4, null, null, null, null, 0, null, null, null, null, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null }
                });

            migrationBuilder.InsertData(
                schema: "Operational",
                table: "AccessPoints",
                columns: new[] { "Id", "Code", "CreateAt", "Description", "Name", "QrCode", "TypeId", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Acceso norte del evento", "Punto Norte", null, 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Acceso sur del evento", "Punto Sur", null, 2, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Acceso principal", "Punto Principal", null, 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Operational",
                table: "EventTargetAudiences",
                columns: new[] { "Id", "Code", "CreateAt", "EventId", "InternalDivisionId", "IsDeleted", "OrganizationalUnitId", "ProfileId", "ReferenceId", "TypeId", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 1, null, false, null, null, 1, 6, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 1, null, false, null, null, 2, 6, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 2, null, false, null, null, 3, 6, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Parameter",
                table: "MenuStructures",
                columns: new[] { "Id", "Code", "CreateAt", "FormId", "Icon", "IsDeleted", "ModuleId", "OrderIndex", "ParentMenuId", "Title", "Type", "UpdateAt" },
                values: new object[,]
                {
                    { 6, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 1, null, false, null, 1, 1, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 2, null, false, null, 2, 1, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null, "account_tree", false, null, 1, 2, "Estructura Organizativa", "collapse", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null, "event_available", false, null, 1, 3, "Eventos y Control de Acceso", "collapse", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 20, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null, "settings_applications", false, null, 1, 4, "Configuración General", "collapse", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 21, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null, "location_on", false, null, 2, 4, "Ubicación", "collapse", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 27, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), null, "admin_panel_settings", false, null, 1, 5, "Gestión de Seguridad", "collapse", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Notifications",
                table: "NotificationReceived",
                columns: new[] { "Id", "Code", "CreateAt", "ExpirationDate", "IsDeleted", "NotificationId", "ReadDate", "SendDate", "StatusId", "UpdateAt", "UserId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 1, 5, 0, 0, 0, DateTimeKind.Utc), false, 1, null, new DateTime(2025, 7, 27, 15, 5, 0, 0, DateTimeKind.Utc), 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 8, 5, 5, 0, 0, 0, DateTimeKind.Utc), false, 2, new DateTime(2025, 7, 28, 15, 15, 0, 0, DateTimeKind.Utc), new DateTime(2025, 7, 28, 14, 35, 0, 0, DateTimeKind.Utc), 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 2 }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "Organizations",
                columns: new[] { "Id", "Code", "CreateAt", "Description", "Logo", "Name", "TypeId", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Institución de educación superior", "logo_unal.png", "Universidad Nacional", 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Centro de atención médica", "logo_hsj.png", "Hospital San José", 2, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "Fundación sin ánimo de lucro", "logo_fundacion.png", "Fundación Futuro", 3, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "PersonDivisionProfiles",
                columns: new[] { "Id", "Code", "CreateAt", "InternalDivisionId", "IsCurrentlySelected", "PersonId", "ProfileId", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 1, true, 1, 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 1, true, 2, 2, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 1, true, 5, 2, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 1, true, 6, 2, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 1, true, 7, 2, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "RolFormPermissions",
                columns: new[] { "Id", "Code", "CreateAt", "FormId", "PermissionId", "RolId", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 1, 1, 3, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 2, 3, 3, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 3, 2, 3, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "UserRoles",
                columns: new[] { "Id", "Code", "CreateAt", "IsDeleted", "RolId", "UpdateAt", "UserId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), false, 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), false, 2, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 2 },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), false, 3, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 3 },
                    { 4, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), false, 4, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 4 },
                    { 5, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), false, 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 2 },
                    { 6, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), false, 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 3 },
                    { 7, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), false, 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 4 }
                });

            migrationBuilder.InsertData(
                schema: "Operational",
                table: "Attendances",
                columns: new[] { "Id", "AccessPointOfEntry", "AccessPointOfExit", "Code", "CreateAt", "PersonId", "QrCode", "TimeOfEntry", "TimeOfExit", "UpdateAt" },
                values: new object[,]
                {
                    { 1, 1, 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 1, null, new DateTime(2023, 1, 1, 13, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 1, 17, 0, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 1, 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 2, null, new DateTime(2023, 1, 1, 14, 30, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 1, 18, 45, 0, 0, DateTimeKind.Utc), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "Branches",
                columns: new[] { "Id", "Address", "CityId", "Code", "CreateAt", "Email", "Location", "Name", "OrganizationId", "Phone", "UpdateAt" },
                values: new object[,]
                {
                    { 1, "Calle 1 # 2-34", 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "principal@org.com", "Centro", "Sucursal Principal", 1, "123456789", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, "Carrera 45 # 67-89", 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), "norte@org.com", "Zona Norte", "Sucursal Norte", 1, "987654321", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "Cards",
                columns: new[] { "Id", "CardTemplateId", "Code", "CreateAt", "CreationDate", "ExpirationDate", "IsDeleted", "PersonDivissionProfileId", "QRCode", "StatusId", "UniqueId", "UpdateAt" },
                values: new object[] { 1, 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), false, 1, "QR12345", 1, new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                schema: "Parameter",
                table: "MenuStructures",
                columns: new[] { "Id", "Code", "CreateAt", "FormId", "Icon", "IsDeleted", "ModuleId", "OrderIndex", "ParentMenuId", "Title", "Type", "UpdateAt" },
                values: new object[,]
                {
                    { 9, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 3, null, false, null, 1, 8, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 4, null, false, null, 2, 8, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 5, null, false, null, 3, 8, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 6, null, false, null, 4, 8, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 7, null, false, null, 5, 8, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 14, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 8, null, false, null, 6, 8, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 16, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 9, null, false, null, 1, 15, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 17, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 10, null, false, null, 2, 15, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 18, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 11, null, false, null, 3, 15, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 19, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 12, null, false, null, 4, 15, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 22, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 13, null, false, null, 1, 20, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 23, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 14, null, false, null, 2, 20, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 24, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 15, null, false, null, 3, 20, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 25, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 16, null, false, null, 1, 21, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 26, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 17, null, false, null, 2, 21, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 28, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 18, null, false, null, 1, 27, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 29, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 19, null, false, null, 2, 27, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 30, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 20, null, false, null, 3, 27, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 31, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 21, null, false, null, 4, 27, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 32, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 22, null, false, null, 5, 27, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 33, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 23, null, false, null, 6, 27, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 34, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 24, null, false, null, 7, 27, null, "item", new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "Schedules",
                columns: new[] { "Id", "Code", "CreateAt", "EndTime", "Name", "OrganizationId", "StartTime", "UpdateAt" },
                values: new object[,]
                {
                    { 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 18, 0, 0, 0), "Horario Jornada A", 1, new TimeSpan(0, 7, 0, 0, 0), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 17, 0, 0, 0), "Horario Jornada B", 1, new TimeSpan(0, 8, 0, 0, 0), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), new TimeSpan(0, 19, 0, 0, 0), "Horario Jornada C", 1, new TimeSpan(0, 6, 30, 0, 0), new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Organizational",
                table: "OrganizationalUnitBranches",
                columns: new[] { "Id", "BranchId", "Code", "CreateAt", "OrganizationUnitId", "UpdateAt" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 1, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 2, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 2, null, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc), 3, new DateTime(1, 1, 1, 5, 0, 0, 0, DateTimeKind.Utc) }
                });

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
                name: "IX_Attendances_IsDeleted",
                schema: "Operational",
                table: "Attendances",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_PersonId",
                schema: "Operational",
                table: "Attendances",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_PersonId_TimeOfExit",
                schema: "Operational",
                table: "Attendances",
                columns: new[] { "PersonId", "TimeOfExit" });

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_TimeOfEntry",
                schema: "Operational",
                table: "Attendances",
                column: "TimeOfEntry");

            migrationBuilder.CreateIndex(
                name: "IX_Attendances_TimeOfExit",
                schema: "Operational",
                table: "Attendances",
                column: "TimeOfExit");

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
                name: "IX_Cards_CardTemplateId",
                schema: "Organizational",
                table: "Cards",
                column: "CardTemplateId");

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
                name: "IX_CardTemplates_Name",
                schema: "Operational",
                table: "CardTemplates",
                column: "Name",
                unique: true);

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
                name: "IX_EventAccessPoints_AccessPointId",
                schema: "Operational",
                table: "EventAccessPoints",
                column: "AccessPointId");

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
                name: "IX_EventTargetAudiences_EventId",
                schema: "Operational",
                table: "EventTargetAudiences",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTargetAudiences_InternalDivisionId",
                schema: "Operational",
                table: "EventTargetAudiences",
                column: "InternalDivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTargetAudiences_OrganizationalUnitId",
                schema: "Operational",
                table: "EventTargetAudiences",
                column: "OrganizationalUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTargetAudiences_ProfileId",
                schema: "Operational",
                table: "EventTargetAudiences",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_EventTargetAudiences_TypeId",
                schema: "Operational",
                table: "EventTargetAudiences",
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
                name: "IX_ImportBatchRows_CardId",
                table: "ImportBatchRows",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportBatchRows_ImportBatchId",
                table: "ImportBatchRows",
                column: "ImportBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportBatchRows_PersonDivisionProfileId",
                table: "ImportBatchRows",
                column: "PersonDivisionProfileId");

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
                name: "IX_Notification_CustomTypeId",
                schema: "Notifications",
                table: "Notification",
                column: "CustomTypeId");

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
                name: "EventAccessPoints",
                schema: "Operational");

            migrationBuilder.DropTable(
                name: "EventTargetAudiences",
                schema: "Operational");

            migrationBuilder.DropTable(
                name: "ImportBatchRows");

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
                name: "Events",
                schema: "Operational");

            migrationBuilder.DropTable(
                name: "Cards",
                schema: "Organizational");

            migrationBuilder.DropTable(
                name: "ImportBatches");

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
                name: "EventTypes",
                schema: "Operational");

            migrationBuilder.DropTable(
                name: "Schedules",
                schema: "Organizational");

            migrationBuilder.DropTable(
                name: "CardTemplates",
                schema: "Operational");

            migrationBuilder.DropTable(
                name: "PersonDivisionProfiles",
                schema: "Organizational");

            migrationBuilder.DropTable(
                name: "Statuses",
                schema: "Parameter");

            migrationBuilder.DropTable(
                name: "Modules",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "InternalDivisions",
                schema: "Organizational");

            migrationBuilder.DropTable(
                name: "People",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "Profiles",
                schema: "Organizational");

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
