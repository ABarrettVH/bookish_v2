using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookishDB.Migrations
{
    /// <inheritdoc />
    public partial class CreateMembersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "HashedPassword",
                table: "Members",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Members",
                newName: "HashedPassword");

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
        }
    }
}
