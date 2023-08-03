using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.Migrations
{
    /// <inheritdoc />
    public partial class userupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Userid",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_Userid",
                table: "Books",
                column: "Userid");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Users_Userid",
                table: "Books",
                column: "Userid",
                principalTable: "Users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Users_Userid",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_Userid",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Userid",
                table: "Books");
        }
    }
}
