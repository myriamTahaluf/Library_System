using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class update4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Libraries_Library_Id",
                table: "Books");

            migrationBuilder.DropTable(
                name: "Library_Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_Library_Id",
                table: "Books");

            migrationBuilder.CreateTable(
                name: "BookLibrary",
                columns: table => new
                {
                    BooksId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    libraryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookLibrary", x => new { x.BooksId, x.libraryId });
                    table.ForeignKey(
                        name: "FK_BookLibrary_Books_BooksId",
                        column: x => x.BooksId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookLibrary_Libraries_libraryId",
                        column: x => x.libraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookLibrary_libraryId",
                table: "BookLibrary",
                column: "libraryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookLibrary");

            migrationBuilder.CreateTable(
                name: "Library_Books",
                columns: table => new
                {
                    Book_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Library_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Library_Books", x => new { x.Book_Id, x.Library_Id });
                    table.ForeignKey(
                        name: "FK_Library_Books_Books_Book_Id",
                        column: x => x.Book_Id,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Library_Books_Libraries_Library_Id",
                        column: x => x.Library_Id,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_Library_Id",
                table: "Books",
                column: "Library_Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Library_Books_Library_Id",
                table: "Library_Books",
                column: "Library_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Libraries_Library_Id",
                table: "Books",
                column: "Library_Id",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
