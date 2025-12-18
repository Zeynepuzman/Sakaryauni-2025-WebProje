using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebProje_B231210095.Migrations
{
    /// <inheritdoc />
    public partial class AddReddetmeSebebiToRandevu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReddetmeSebebi",
                table: "Randevular",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReddetmeSebebi",
                table: "Randevular");
        }
    }
}
