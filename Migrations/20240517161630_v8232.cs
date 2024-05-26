using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yetai_Eats.Migrations
{
    /// <inheritdoc />
    public partial class v8232 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserProfile",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserProfile",
                table: "AspNetUsers");
        }
    }
}
