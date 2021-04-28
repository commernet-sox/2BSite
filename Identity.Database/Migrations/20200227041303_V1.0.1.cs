using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Identity.Database.Migrations
{
    public partial class V101 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSystem",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ModifyTime = table.Column<DateTime>(nullable: true),
                    Modifier = table.Column<string>(maxLength: 150, nullable: true),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    Creator = table.Column<string>(maxLength: 150, nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    SystemId = table.Column<int>(nullable: false),
                    Remarks = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSystem", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSystem");
        }
    }
}
