using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TicTacToe.DataAccess.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "Game_Id_HiLo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "Turn_Id_HiLo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "tbGame",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    EndedAt = table.Column<DateTime>(nullable: true),
                    IsPlayerFirst = table.Column<bool>(nullable: false),
                    StartedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbGame", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tbTurn",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    GameId = table.Column<int>(nullable: false),
                    X = table.Column<int>(nullable: false),
                    Y = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbTurn", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tbTurn_tbGame_GameId",
                        column: x => x.GameId,
                        principalTable: "tbGame",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbTurn_GameId",
                table: "tbTurn",
                column: "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbTurn");

            migrationBuilder.DropTable(
                name: "tbGame");

            migrationBuilder.DropSequence(
                name: "Game_Id_HiLo");

            migrationBuilder.DropSequence(
                name: "Turn_Id_HiLo");
        }
    }
}
