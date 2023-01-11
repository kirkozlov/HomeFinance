using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeFinance.DataAccess.Sqlite.Migrations
{
    public partial class tagSortId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SortId",
                table: "Tags",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SortId",
                table: "Tags");
        }
    }
}
