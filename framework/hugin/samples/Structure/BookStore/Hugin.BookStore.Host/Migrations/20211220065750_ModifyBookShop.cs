using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hugin.BookStore.Migrations
{
    public partial class ModifyBookShop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "BookStore_BookShop");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BookStore_BookShop");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "BookStore_BookShop",
                type: "char(36)",
                nullable: true,
                comment: "租户Id");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "BookStore_BookShop",
                type: "char(36)",
                nullable: true,
                comment: "UserId");
        }
    }
}
