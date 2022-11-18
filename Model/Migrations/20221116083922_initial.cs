using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Full_Name = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Book_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Library_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Reserved_User_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Is_Available_To_Reserve = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Users_Reserved_User_Id",
                        column: x => x.Reserved_User_Id,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Libraries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(26)", maxLength: 26, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Book_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libraries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Libraries_Books_Book_Id",
                        column: x => x.Book_Id,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_Library_Id",
                table: "Books",
                column: "Library_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Reserved_User_Id",
                table: "Books",
                column: "Reserved_User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_Book_Id",
                table: "Libraries",
                column: "Book_Id");

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
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Libraries_Library_Id",
                table: "Books");

            migrationBuilder.DropTable(
                name: "Library_Books");

            migrationBuilder.DropTable(
                name: "Libraries");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
