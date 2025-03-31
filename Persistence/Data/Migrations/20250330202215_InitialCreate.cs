using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Topic",
                columns: table => new
                {
                    Id_Topic = table.Column<byte[]>(type: "binary(16)", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Username = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<byte[]>(type: "binary(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topic", x => x.Id_Topic);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Idea",
                columns: table => new
                {
                    Id_Idea = table.Column<byte[]>(type: "binary(16)", nullable: false),
                    TopicId = table.Column<byte[]>(type: "binary(16)", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Username = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<byte[]>(type: "binary(16)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Idea", x => x.Id_Idea);
                    table.ForeignKey(
                        name: "FK_Idea_Topic_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topic",
                        principalColumn: "Id_Topic",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Vote",
                columns: table => new
                {
                    Id_Vote = table.Column<byte[]>(type: "binary(16)", nullable: false),
                    UserId = table.Column<byte[]>(type: "binary(16)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    VotedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    IdeaId = table.Column<byte[]>(type: "binary(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vote", x => x.Id_Vote);
                    table.ForeignKey(
                        name: "FK_Vote_Idea_IdeaId",
                        column: x => x.IdeaId,
                        principalTable: "Idea",
                        principalColumn: "Id_Idea",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Topic",
                columns: new[] { "Id_Topic", "CreatedAt", "Description", "Title", "UserId", "Username" },
                values: new object[,]
                {
                    { new byte[] { 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17 }, new DateTime(2025, 3, 30, 20, 22, 11, 673, DateTimeKind.Utc).AddTicks(1379), "Discussion on implementing CQRS in a microservices architecture.", "How to implement CQRS", new byte[] { 113, 176, 8, 226, 252, 214, 23, 65, 171, 174, 9, 61, 192, 66, 8, 100 }, "angelito_374" },
                    { new byte[] { 34, 34, 34, 34, 34, 34, 34, 34, 34, 34, 34, 34, 34, 34, 34, 34 }, new DateTime(2025, 3, 30, 20, 22, 11, 673, DateTimeKind.Utc).AddTicks(1381), "Best practices for building clean and maintainable software architectures.", "Best practices for clean architecture", new byte[] { 113, 176, 8, 226, 252, 214, 23, 65, 171, 174, 9, 61, 192, 66, 8, 100 }, "angelito_374" }
                });

            migrationBuilder.InsertData(
                table: "Idea",
                columns: new[] { "Id_Idea", "CreatedAt", "Description", "Title", "TopicId", "UserId", "Username" },
                values: new object[,]
                {
                    { new byte[] { 51, 51, 51, 51, 51, 51, 51, 51, 51, 51, 51, 51, 51, 51, 51, 51 }, new DateTime(2025, 3, 30, 20, 22, 11, 672, DateTimeKind.Utc).AddTicks(9115), "Discussing how caching can improve performance in CQRS systems.", "Implementing caching in CQRS", new byte[] { 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17 }, new byte[] { 129, 110, 65, 68, 0, 221, 54, 67, 132, 1, 225, 69, 124, 210, 207, 158 }, "catriel_72" },
                    { new byte[] { 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68 }, new DateTime(2025, 3, 30, 20, 22, 11, 672, DateTimeKind.Utc).AddTicks(9117), "Exploring the usage of domain events in CQRS systems.", "Domain events in CQRS", new byte[] { 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17, 17 }, new byte[] { 129, 110, 65, 68, 0, 221, 54, 67, 132, 1, 225, 69, 124, 210, 207, 158 }, "catriel_72" }
                });

            migrationBuilder.InsertData(
                table: "Vote",
                columns: new[] { "Id_Vote", "IdeaId", "UserId", "Value", "VotedAt" },
                values: new object[,]
                {
                    { new byte[] { 65, 131, 100, 214, 155, 199, 201, 67, 141, 7, 161, 6, 217, 61, 127, 136 }, new byte[] { 51, 51, 51, 51, 51, 51, 51, 51, 51, 51, 51, 51, 51, 51, 51, 51 }, new byte[] { 236, 60, 215, 34, 93, 60, 33, 72, 175, 106, 33, 195, 252, 53, 104, 202 }, 1, new DateTime(2025, 3, 30, 20, 22, 11, 673, DateTimeKind.Utc).AddTicks(4106) },
                    { new byte[] { 210, 149, 162, 115, 240, 217, 217, 72, 135, 70, 101, 79, 10, 166, 23, 150 }, new byte[] { 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68, 68 }, new byte[] { 236, 60, 215, 34, 93, 60, 33, 72, 175, 106, 33, 195, 252, 53, 104, 202 }, -1, new DateTime(2025, 3, 30, 20, 22, 11, 673, DateTimeKind.Utc).AddTicks(4114) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Idea_TopicId",
                table: "Idea",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_IdeaId",
                table: "Vote",
                column: "IdeaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vote");

            migrationBuilder.DropTable(
                name: "Idea");

            migrationBuilder.DropTable(
                name: "Topic");
        }
    }
}
