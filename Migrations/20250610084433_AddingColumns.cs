using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookishDB.Migrations
{
    /// <inheritdoc />
    public partial class AddingColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookViewModelBookID",
                table: "Books",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookViewModelBookID",
                table: "Books",
                column: "BookViewModelBookID");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Books_BookViewModelBookID",
                table: "Books",
                column: "BookViewModelBookID",
                principalTable: "Books",
                principalColumn: "BookID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Books_BookViewModelBookID",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookViewModelBookID",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookViewModelBookID",
                table: "Books");
        }
    }
}
