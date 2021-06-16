using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WXSite.Database.Migrations
{
    public partial class V10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "SEQ_ORDER",
                minValue: 1L,
                maxValue: 999999L);

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
                name: "CodeMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(maxLength: 150, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(maxLength: 150, nullable: true),
                    CodeGroup = table.Column<string>(maxLength: 20, nullable: false),
                    CodeId = table.Column<string>(maxLength: 20, nullable: false),
                    CodeName = table.Column<string>(maxLength: 50, nullable: true),
                    IsActive = table.Column<string>(type: "char(1)", maxLength: 1, nullable: true),
                    Remarks = table.Column<string>(maxLength: 200, nullable: true),
                    HUDF_01 = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Error",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(maxLength: 150, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(maxLength: 150, nullable: true),
                    QuestionList = table.Column<string>(nullable: true),
                    QuestionMenu = table.Column<string>(nullable: true),
                    MenuId = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Error", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedBack",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(maxLength: 150, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(maxLength: 150, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedBack", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "History",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(maxLength: 150, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(maxLength: 150, nullable: true),
                    QuestionList = table.Column<string>(nullable: true),
                    QuestionMenu = table.Column<string>(nullable: true),
                    MenuId = table.Column<string>(nullable: true),
                    Score = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_History", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuestionMenu",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(maxLength: 150, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(maxLength: 150, nullable: true),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Time = table.Column<int>(nullable: false),
                    PeopleNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionMenu", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(maxLength: 150, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(maxLength: 150, nullable: true),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    Help = table.Column<string>(maxLength: 500, nullable: true),
                    Type = table.Column<string>(maxLength: 10, nullable: false),
                    ChoseList = table.Column<string>(maxLength: 500, nullable: false),
                    MenuId = table.Column<int>(nullable: false),
                    PicUrl = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(maxLength: 150, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(maxLength: 150, nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    MobilePhoneNumber = table.Column<string>(nullable: true),
                    NickName = table.Column<string>(nullable: true),
                    AvatarUrl = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    AuthData = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
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
                name: "CodeMaster");

            migrationBuilder.DropTable(
                name: "Error");

            migrationBuilder.DropTable(
                name: "FeedBack");

            migrationBuilder.DropTable(
                name: "History");

            migrationBuilder.DropTable(
                name: "QuestionMenu");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "AuditEntry");

            migrationBuilder.DropSequence(
                name: "SEQ_ORDER");
        }
    }
}
