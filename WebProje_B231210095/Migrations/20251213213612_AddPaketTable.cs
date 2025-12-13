using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebProje_B231210095.Migrations
{
    /// <inheritdoc />
    public partial class AddPaketTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SalonId",
                table: "Paketler",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalonId",
                table: "Paketler");
        }
    }
}
