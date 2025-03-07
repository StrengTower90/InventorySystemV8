using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventarySystem.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AlterSerialNumberInProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SerieNumber",
                table: "Product",
                newName: "SerialNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SerialNumber",
                table: "Product",
                newName: "SerieNumber");
        }
    }
}
