using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Gazelle.Migrations
{
    public partial class CityProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EndCityCityId",
                table: "Connections",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StartCityCityId",
                table: "Connections",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Connections_EndCityCityId",
                table: "Connections",
                column: "EndCityCityId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_StartCityCityId",
                table: "Connections",
                column: "StartCityCityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Cities_EndCityCityId",
                table: "Connections",
                column: "EndCityCityId",
                principalTable: "Cities",
                principalColumn: "CityId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Connections_Cities_StartCityCityId",
                table: "Connections",
                column: "StartCityCityId",
                principalTable: "Cities",
                principalColumn: "CityId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Cities_EndCityCityId",
                table: "Connections");

            migrationBuilder.DropForeignKey(
                name: "FK_Connections_Cities_StartCityCityId",
                table: "Connections");

            migrationBuilder.DropIndex(
                name: "IX_Connections_EndCityCityId",
                table: "Connections");

            migrationBuilder.DropIndex(
                name: "IX_Connections_StartCityCityId",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "EndCityCityId",
                table: "Connections");

            migrationBuilder.DropColumn(
                name: "StartCityCityId",
                table: "Connections");
        }
    }
}
