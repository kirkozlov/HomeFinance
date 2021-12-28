using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeFinance.Migrations
{
    public partial class fixUserIdForeigKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Operations_Categories_CategoryId",
                table: "Operations");

            migrationBuilder.AddColumn<string>(
                name: "HomeFinanceUserId",
                table: "Wallets",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Operations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "HomeFinanceUserId",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_HomeFinanceUserId",
                table: "Wallets",
                column: "HomeFinanceUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_HomeFinanceUserId",
                table: "Categories",
                column: "HomeFinanceUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_HomeFinanceUserId",
                table: "Categories",
                column: "HomeFinanceUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_Categories_CategoryId",
                table: "Operations",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_AspNetUsers_HomeFinanceUserId",
                table: "Wallets",
                column: "HomeFinanceUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_HomeFinanceUserId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Operations_Categories_CategoryId",
                table: "Operations");

            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_AspNetUsers_HomeFinanceUserId",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_HomeFinanceUserId",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Categories_HomeFinanceUserId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "HomeFinanceUserId",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "HomeFinanceUserId",
                table: "Categories");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Operations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Operations_Categories_CategoryId",
                table: "Operations",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
