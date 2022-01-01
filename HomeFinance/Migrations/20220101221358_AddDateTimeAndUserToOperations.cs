using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeFinance.Migrations
{
    public partial class AddDateTimeAndUserToOperations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateTime",
                table: "Operations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "HomeFinanceUserId",
                table: "Operations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_HomeFinanceUserId",
                table: "Operations",
                column: "HomeFinanceUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_AspNetUsers_HomeFinanceUserId",
                table: "Operations",
                column: "HomeFinanceUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_AspNetUsers_HomeFinanceUserId",
                table: "Operations");

            migrationBuilder.DropIndex(
                name: "IX_Operations_HomeFinanceUserId",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "DateTime",
                table: "Operations");

            migrationBuilder.DropColumn(
                name: "HomeFinanceUserId",
                table: "Operations");
        }
    }
}
