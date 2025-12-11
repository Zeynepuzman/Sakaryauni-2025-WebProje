using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebProje_B231210095.Migrations
{
    /// <inheritdoc />
    public partial class RemoveBiyografi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Biyografi",
                table: "Antrenorler");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Biyografi",
                table: "Antrenorler",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
