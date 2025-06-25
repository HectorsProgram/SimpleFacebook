using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleFacebook.Migrations
{
    /// <inheritdoc />
    public partial class RenamePostIdToId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "Posts",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "Username",
                table: "Likes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Likes");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Posts",
                newName: "PostId");
        }
    }
}
