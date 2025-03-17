using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarySystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddKardexInventoryMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KardexInventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreProductId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreviousStock = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<double>(type: "float", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    UserApplicationId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KardexInventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KardexInventory_AspNetUsers_UserApplicationId",
                        column: x => x.UserApplicationId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KardexInventory_StoresProducts_StoreProductId",
                        column: x => x.StoreProductId,
                        principalTable: "StoresProducts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_KardexInventory_StoreProductId",
                table: "KardexInventory",
                column: "StoreProductId");

            migrationBuilder.CreateIndex(
                name: "IX_KardexInventory_UserApplicationId",
                table: "KardexInventory",
                column: "UserApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KardexInventory");
        }
    }
}
