using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYPTourneyPro.Migrations
{
    /// <inheritdoc />
    public partial class Remove_PRegTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryParticipant_Category_CategoryId",
                table: "CategoryParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryParticipant_PlayerRegistration_PlayerRegistrationId",
                table: "CategoryParticipant");

            migrationBuilder.DropTable(
                name: "PlayerRegistration");

            migrationBuilder.DropIndex(
                name: "IX_CategoryParticipant_CategoryId",
                table: "CategoryParticipant");

            migrationBuilder.DropIndex(
                name: "IX_CategoryParticipant_PlayerRegistrationId",
                table: "CategoryParticipant");

            migrationBuilder.RenameColumn(
                name: "PlayerRegistrationId",
                table: "CategoryParticipant",
                newName: "ParticipantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParticipantId",
                table: "CategoryParticipant",
                newName: "PlayerRegistrationId");

            migrationBuilder.CreateTable(
                name: "PlayerRegistration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TournamentId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerRegistration", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryParticipant_CategoryId",
                table: "CategoryParticipant",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryParticipant_PlayerRegistrationId",
                table: "CategoryParticipant",
                column: "PlayerRegistrationId");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryParticipant_Category_CategoryId",
                table: "CategoryParticipant",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryParticipant_PlayerRegistration_PlayerRegistrationId",
                table: "CategoryParticipant",
                column: "PlayerRegistrationId",
                principalTable: "PlayerRegistration",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
