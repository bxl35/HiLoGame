using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HiloGame.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    MinRange = table.Column<int>(type: "integer", nullable: false),
                    MaxRange = table.Column<int>(type: "integer", nullable: false),
                    MysteryNumber = table.Column<int>(type: "integer", nullable: false),
                    GuessCount = table.Column<int>(type: "integer", nullable: false),
                    IsGameWon = table.Column<bool>(type: "boolean", nullable: false),
                    IsGameOver = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
