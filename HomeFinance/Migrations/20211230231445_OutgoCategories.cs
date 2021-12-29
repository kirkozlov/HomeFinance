using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeFinance.Migrations
{
    public partial class OutgoCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Outgo",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Outgo",
                table: "Categories");
        }
    }
}
