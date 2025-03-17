using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarySystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryAlterNameMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_AspNetUsers_UserApplicationId",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Stores_StoreId",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryDetails_Inventory_InventoryId",
                table: "InventoryDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inventory",
                table: "Inventory");

            migrationBuilder.RenameTable(
                name: "Inventory",
                newName: "Inventories");

            migrationBuilder.RenameIndex(
                name: "IX_Inventory_UserApplicationId",
                table: "Inventories",
                newName: "IX_Inventories_UserApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventory_StoreId",
                table: "Inventories",
                newName: "IX_Inventories_StoreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inventories",
                table: "Inventories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_AspNetUsers_UserApplicationId",
                table: "Inventories",
                column: "UserApplicationId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_Stores_StoreId",
                table: "Inventories",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryDetails_Inventories_InventoryId",
                table: "InventoryDetails",
                column: "InventoryId",
                principalTable: "Inventories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_AspNetUsers_UserApplicationId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_Stores_StoreId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryDetails_Inventories_InventoryId",
                table: "InventoryDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inventories",
                table: "Inventories");

            migrationBuilder.RenameTable(
                name: "Inventories",
                newName: "Inventory");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_UserApplicationId",
                table: "Inventory",
                newName: "IX_Inventory_UserApplicationId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_StoreId",
                table: "Inventory",
                newName: "IX_Inventory_StoreId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inventory",
                table: "Inventory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_AspNetUsers_UserApplicationId",
                table: "Inventory",
                column: "UserApplicationId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Stores_StoreId",
                table: "Inventory",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryDetails_Inventory_InventoryId",
                table: "InventoryDetails",
                column: "InventoryId",
                principalTable: "Inventory",
                principalColumn: "Id");
        }
    }
}
