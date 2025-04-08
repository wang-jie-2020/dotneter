using Microsoft.EntityFrameworkCore.Migrations;

namespace Hugin.IdentityServer.Migrations
{
    public partial class AppendAppUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardNo",
                table: "AbpUsers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AbpUsers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardNo",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AbpUsers");
        }
    }
}
