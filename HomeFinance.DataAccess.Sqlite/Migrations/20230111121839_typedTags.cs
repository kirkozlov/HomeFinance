using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeFinance.DataAccess.Sqlite.Migrations
{
    public partial class typedTags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Tags");

            migrationBuilder.AddColumn<int>(
                name: "OperationType",
                table: "Tags",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OperationType",
                table: "Tags");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Tags",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
