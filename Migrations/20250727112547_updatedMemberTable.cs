using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookishDB.Migrations
{
    /// <inheritdoc />
    public partial class updatedMemberTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberViewModelMemberID",
                table: "Members",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostCode",
                table: "Members",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_MemberViewModelMemberID",
                table: "Members",
                column: "MemberViewModelMemberID");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Members_MemberViewModelMemberID",
                table: "Members",
                column: "MemberViewModelMemberID",
                principalTable: "Members",
                principalColumn: "MemberID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Members_MemberViewModelMemberID",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_MemberViewModelMemberID",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MemberViewModelMemberID",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PostCode",
                table: "Members");
        }
    }
}
