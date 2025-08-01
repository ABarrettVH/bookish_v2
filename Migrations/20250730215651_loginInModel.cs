using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookishDB.Migrations
{
    /// <inheritdoc />
    public partial class loginInModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HashedPassword",
                table: "Members",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Members",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "Salt",
                table: "Members",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Members",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HashedPassword",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Members");
        }
    }
}
