using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SailorNumberGuessingGame.Server.Migrations
{
    public partial class Misc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExpectedNumber",
                table: "Games",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "GameEnded",
                table: "Games",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "GameStarted",
                table: "Games",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "successful",
                table: "Games",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "NumberEntered",
                table: "Actions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfBuoys",
                table: "Actions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfShips",
                table: "Actions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Games_PlayerId",
                table: "Games",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Actions_GameId",
                table: "Actions",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actions_Games_GameId",
                table: "Actions",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Players_PlayerId",
                table: "Games",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actions_Games_GameId",
                table: "Actions");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_Players_PlayerId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_PlayerId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Actions_GameId",
                table: "Actions");

            migrationBuilder.DropColumn(
                name: "ExpectedNumber",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GameEnded",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "GameStarted",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "successful",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "NumberEntered",
                table: "Actions");

            migrationBuilder.DropColumn(
                name: "NumberOfBuoys",
                table: "Actions");

            migrationBuilder.DropColumn(
                name: "NumberOfShips",
                table: "Actions");
        }
    }
}
