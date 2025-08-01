using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookishDB.Migrations
{
    /// <inheritdoc />
    public partial class addingSalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Salt",
                table: "Members",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Members");
        }
    }
}
