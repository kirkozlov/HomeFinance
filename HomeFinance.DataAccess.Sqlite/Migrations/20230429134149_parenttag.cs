using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeFinance.DataAccess.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class parenttag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ParentTagName",
                table: "Tags",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ParentTagOperationType",
                table: "Tags",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ParentTagName_ParentTagOperationType",
                table: "Tags",
                columns: new[] { "ParentTagName", "ParentTagOperationType" });

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Tags_ParentTagName_ParentTagOperationType",
                table: "Tags",
                columns: new[] { "ParentTagName", "ParentTagOperationType" },
                principalTable: "Tags",
                principalColumns: new[] { "Name", "OperationType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Tags_ParentTagName_ParentTagOperationType",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_ParentTagName_ParentTagOperationType",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ParentTagName",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "ParentTagOperationType",
                table: "Tags");
        }
    }
}
