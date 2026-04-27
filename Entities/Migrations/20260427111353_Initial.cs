using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "peoples",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReceiveNewsLetters = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_peoples", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "countries",
                columns: new[] { "Id", "CountryName" },
                values: new object[,]
                {
                    { new Guid("06706ac8-6f04-44eb-8e02-c545dbad1633"), "Norway" },
                    { new Guid("40ea2121-b73c-4dce-8d95-368846a15abe"), "Estonia" },
                    { new Guid("7f516b57-8115-4bc6-8720-ec7eae4f93c8"), "Finland" },
                    { new Guid("ce4fb821-68f4-4c06-b16f-7d44647f2d41"), "China" }
                });

            migrationBuilder.InsertData(
                table: "peoples",
                columns: new[] { "Id", "Adress", "CountryId", "DateOfBirth", "Email", "Gender", "PersonName", "ReceiveNewsLetters" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Helsinki, Finland", new Guid("7f516b57-8115-4bc6-8720-ec7eae4f93c8"), new DateTime(1990, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "john.smith@example.com", "Male", "John Smith", false },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Shanghai, China", new Guid("ce4fb821-68f4-4c06-b16f-7d44647f2d41"), new DateTime(1988, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "li.wei@example.com", "Male", "Li Wei", true },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Tallinn, Estonia", new Guid("40ea2121-b73c-4dce-8d95-368846a15abe"), new DateTime(1997, 11, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "anna.kask@example.com", "Female", "Anna Kask", false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "countries");

            migrationBuilder.DropTable(
                name: "peoples");
        }
    }
}
