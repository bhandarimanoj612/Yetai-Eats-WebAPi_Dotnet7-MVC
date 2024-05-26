using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yetai_Eats.Migrations
{
    /// <inheritdoc />
    public partial class v334 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiliveryRIders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SellerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RiderName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiliveryStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiliveredTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiliveryRIders", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiliveryRIders");
        }
    }
}
