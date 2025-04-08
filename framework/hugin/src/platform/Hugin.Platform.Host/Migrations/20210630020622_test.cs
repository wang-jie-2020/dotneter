using Microsoft.EntityFrameworkCore.Migrations;

namespace Hugin.Platform.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Platform_Stadium",
                table: "Platform_Stadium");

            migrationBuilder.RenameTable(
                name: "Platform_Stadium",
                newName: "platform_stadium");

            migrationBuilder.AddPrimaryKey(
                name: "PK_platform_stadium",
                table: "platform_stadium",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_platform_stadium",
                table: "platform_stadium");

            migrationBuilder.RenameTable(
                name: "platform_stadium",
                newName: "Platform_Stadium");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Platform_Stadium",
                table: "Platform_Stadium",
                column: "Id");
        }
    }
}
