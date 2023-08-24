using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping.Migrations
{
    /// <inheritdoc />
    public partial class cart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookUsers_Books_bookId",
                table: "BookUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_BookUsers_Users_userId",
                table: "BookUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookUsers",
                table: "BookUsers");

            migrationBuilder.RenameTable(
                name: "BookUsers",
                newName: "bookUsers");

            migrationBuilder.RenameIndex(
                name: "IX_BookUsers_userId",
                table: "bookUsers",
                newName: "IX_bookUsers_userId");

            migrationBuilder.RenameIndex(
                name: "IX_BookUsers_bookId",
                table: "bookUsers",
                newName: "IX_bookUsers_bookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookUsers",
                table: "bookUsers",
                column: "id");

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartBooks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    bookId = table.Column<int>(type: "int", nullable: false),
                    cartId = table.Column<int>(type: "int", nullable: false),
                    WantedCopies = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartBooks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartBooks_Books_bookId",
                        column: x => x.bookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartBooks_Carts_cartId",
                        column: x => x.cartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartBooks_bookId",
                table: "CartBooks",
                column: "bookId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartBooks_cartId",
                table: "CartBooks",
                column: "cartId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_bookUsers_Books_bookId",
                table: "bookUsers",
                column: "bookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bookUsers_Users_userId",
                table: "bookUsers",
                column: "userId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookUsers_Books_bookId",
                table: "bookUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_bookUsers_Users_userId",
                table: "bookUsers");

            migrationBuilder.DropTable(
                name: "CartBooks");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bookUsers",
                table: "bookUsers");

            migrationBuilder.RenameTable(
                name: "bookUsers",
                newName: "BookUsers");

            migrationBuilder.RenameIndex(
                name: "IX_bookUsers_userId",
                table: "BookUsers",
                newName: "IX_BookUsers_userId");

            migrationBuilder.RenameIndex(
                name: "IX_bookUsers_bookId",
                table: "BookUsers",
                newName: "IX_BookUsers_bookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookUsers",
                table: "BookUsers",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookUsers_Books_bookId",
                table: "BookUsers",
                column: "bookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookUsers_Users_userId",
                table: "BookUsers",
                column: "userId",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
