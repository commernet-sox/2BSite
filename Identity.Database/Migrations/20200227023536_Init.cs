using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.Database.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditEntry",
                columns: table => new
                {
                    AuditEntryID = table.Column<string>(maxLength: 50, nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    EntitySetName = table.Column<string>(maxLength: 255, nullable: true),
                    EntityTypeName = table.Column<string>(maxLength: 255, nullable: true),
                    State = table.Column<int>(nullable: false),
                    StateName = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntry", x => x.AuditEntryID);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(maxLength: 150, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(maxLength: 150, nullable: true),
                    Name = table.Column<string>(maxLength: 150, nullable: false),
                    Control = table.Column<string>(maxLength: 150, nullable: false),
                    Action = table.Column<string>(maxLength: 150, nullable: false),
                    Area = table.Column<string>(maxLength: 150, nullable: true),
                    OrderIndex = table.Column<int>(nullable: true),
                    ParentID = table.Column<int>(nullable: true),
                    Remarks = table.Column<string>(maxLength: 500, nullable: true),
                    SystemID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(maxLength: 150, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(maxLength: 150, nullable: true),
                    Name = table.Column<string>(maxLength: 150, nullable: true),
                    Remarks = table.Column<string>(maxLength: 500, nullable: true),
                    SystemID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(maxLength: 150, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(maxLength: 150, nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    PermissionId = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "System",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(maxLength: 150, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(maxLength: 150, nullable: true),
                    Name = table.Column<string>(maxLength: 150, nullable: true),
                    Remarks = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_System", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(maxLength: 150, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(maxLength: 150, nullable: true),
                    AliasName = table.Column<string>(maxLength: 150, nullable: true),
                    LoginName = table.Column<string>(maxLength: 150, nullable: true),
                    Password = table.Column<string>(maxLength: 150, nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false),
                    LastLoginDatetime = table.Column<DateTime>(nullable: true),
                    LoginNumber = table.Column<int>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: false),
                    CompanyId = table.Column<int>(nullable: true),
                    Company = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(maxLength: 150, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(maxLength: 150, nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditEntryProperty",
                columns: table => new
                {
                    AuditEntryPropertyID = table.Column<string>(maxLength: 50, nullable: false),
                    AuditEntryID = table.Column<string>(maxLength: 50, nullable: true),
                    PropertyName = table.Column<string>(maxLength: 255, nullable: true),
                    RelationName = table.Column<string>(maxLength: 255, nullable: true),
                    NewValue = table.Column<string>(nullable: true),
                    OldValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntryProperty", x => x.AuditEntryPropertyID);
                    table.ForeignKey(
                        name: "FK_AuditEntryProperty_AuditEntry_AuditEntryID",
                        column: x => x.AuditEntryID,
                        principalTable: "AuditEntry",
                        principalColumn: "AuditEntryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntryProperty_AuditEntryID",
                table: "AuditEntryProperty",
                column: "AuditEntryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditEntryProperty");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DropTable(
                name: "System");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "AuditEntry");
        }
    }
}
