using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yetai_Eats.Migrations
{
    /// <inheritdoc />
    public partial class v620 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "DiliveryRIders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "DiliveryRIders");
        }
    }
}
