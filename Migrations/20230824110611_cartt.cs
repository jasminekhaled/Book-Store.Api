using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.Migrations
{
    /// <inheritdoc />
    public partial class cartt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CartBooks_bookId",
                table: "CartBooks");

            migrationBuilder.CreateIndex(
                name: "IX_CartBooks_bookId",
                table: "CartBooks",
                column: "bookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CartBooks_bookId",
                table: "CartBooks");

            migrationBuilder.CreateIndex(
                name: "IX_CartBooks_bookId",
                table: "CartBooks",
                column: "bookId",
                unique: true);
        }
    }
}
