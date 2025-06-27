using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleFacebook.Migrations
{
    /// <inheritdoc />
    public partial class AddFriendCountToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FriendCount",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FriendCount",
                table: "Users");
        }
    }
}
