using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class update3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Libraries_Books_Book_Id",
                table: "Libraries");

            migrationBuilder.DropIndex(
                name: "IX_Libraries_Book_Id",
                table: "Libraries");

            migrationBuilder.DropIndex(
                name: "IX_Books_Library_Id",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Book_Id",
                table: "Libraries");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Library_Id",
                table: "Books",
                column: "Library_Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_Library_Id",
                table: "Books");

            migrationBuilder.AddColumn<Guid>(
                name: "Book_Id",
                table: "Libraries",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Libraries_Book_Id",
                table: "Libraries",
                column: "Book_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Books_Library_Id",
                table: "Books",
                column: "Library_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Libraries_Books_Book_Id",
                table: "Libraries",
                column: "Book_Id",
                principalTable: "Books",
                principalColumn: "Id");
        }
    }
}
