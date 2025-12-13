using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebProje_B231210095.Migrations
{
    /// <inheritdoc />
    public partial class AddUyePaket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UyePaketler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UyeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaketId = table.Column<int>(type: "int", nullable: false),
                    BaslangicTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BitisTarihi = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AktifMi = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UyePaketler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UyePaketler_AspNetUsers_UyeId",
                        column: x => x.UyeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UyePaketler_Paketler_PaketId",
                        column: x => x.PaketId,
                        principalTable: "Paketler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UyePaketler_PaketId",
                table: "UyePaketler",
                column: "PaketId");

            migrationBuilder.CreateIndex(
                name: "IX_UyePaketler_UyeId",
                table: "UyePaketler",
                column: "UyeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UyePaketler");
        }
    }
}
