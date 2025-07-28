using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookishDB.Migrations
{
    /// <inheritdoc />
    public partial class MemberTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberViewModelMemberID",
                table: "Members",
                type: "integer",
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
    }
}
