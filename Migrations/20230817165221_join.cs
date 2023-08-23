using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.Migrations
{
    /// <inheritdoc />
    public partial class join : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "BookUsers",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bookId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookUsers", x => x.id);
                    table.ForeignKey(
                        name: "FK_BookUsers_Books_bookId",
                        column: x => x.bookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookUsers_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookUsers_bookId",
                table: "BookUsers",
                column: "bookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookUsers_userId",
                table: "BookUsers",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookUsers");

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
    }
}
