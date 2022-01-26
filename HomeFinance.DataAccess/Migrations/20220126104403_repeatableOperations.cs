using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeFinance.DataAccess.Migrations
{
    public partial class repeatableOperations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RepeatableOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HomeFinanceUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OperationType = table.Column<int>(type: "int", nullable: false),
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    WalletIdTo = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepeatableOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepeatableOperations_AspNetUsers_HomeFinanceUserId",
                        column: x => x.HomeFinanceUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepeatableOperations_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RepeatableOperations_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RepeatableOperations_Wallets_WalletIdTo",
                        column: x => x.WalletIdTo,
                        principalTable: "Wallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepeatableOperations_CategoryId",
                table: "RepeatableOperations",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_RepeatableOperations_HomeFinanceUserId",
                table: "RepeatableOperations",
                column: "HomeFinanceUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RepeatableOperations_WalletId",
                table: "RepeatableOperations",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_RepeatableOperations_WalletIdTo",
                table: "RepeatableOperations",
                column: "WalletIdTo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepeatableOperations");
        }
    }
}
