using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookishDB.Migrations
{
    /// <inheritdoc />
    public partial class checkedOutBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookViewModelBookID",
                table: "Members",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_BookViewModelBookID",
                table: "Members",
                column: "BookViewModelBookID");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Books_BookViewModelBookID",
                table: "Members",
                column: "BookViewModelBookID",
                principalTable: "Books",
                principalColumn: "BookID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Books_BookViewModelBookID",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_BookViewModelBookID",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "BookViewModelBookID",
                table: "Members");
        }
    }
}
