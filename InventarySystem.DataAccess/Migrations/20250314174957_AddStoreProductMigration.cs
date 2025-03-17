using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarySystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreProductMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoresProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoresProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoresProducts_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StoresProducts_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoresProducts_ProductId",
                table: "StoresProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StoresProducts_StoreId",
                table: "StoresProducts",
                column: "StoreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoresProducts");
        }
    }
}
