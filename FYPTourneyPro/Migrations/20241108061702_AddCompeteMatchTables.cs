﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYPTourneyPro.Migrations
{
    /// <inheritdoc />
    public partial class AddCompeteMatchTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Compete",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Player1Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Player2Id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Compete", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Compete_CategoryParticipant_Player1Id",
                        column: x => x.Player1Id,
                        principalTable: "CategoryParticipant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Compete_CategoryParticipant_Player2Id",
                        column: x => x.Player2Id,
                        principalTable: "CategoryParticipant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Compete_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompeteId = table.Column<Guid>(type: "uuid", nullable: false),
                    MatchTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Court = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Match_Compete_CompeteId",
                        column: x => x.CompeteId,
                        principalTable: "Compete",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryParticipant_CategoryId",
                table: "CategoryParticipant",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryParticipant_PlayerRegistrationId",
                table: "CategoryParticipant",
                column: "PlayerRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Compete_CategoryId",
                table: "Compete",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Compete_Player1Id",
                table: "Compete",
                column: "Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Compete_Player2Id",
                table: "Compete",
                column: "Player2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Match_CompeteId",
                table: "Match",
                column: "CompeteId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryParticipant_Category_CategoryId",
                table: "CategoryParticipant");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoryParticipant_PlayerRegistration_PlayerRegistrationId",
                table: "CategoryParticipant");

            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.DropTable(
                name: "Compete");

            migrationBuilder.DropIndex(
                name: "IX_CategoryParticipant_CategoryId",
                table: "CategoryParticipant");

            migrationBuilder.DropIndex(
                name: "IX_CategoryParticipant_PlayerRegistrationId",
                table: "CategoryParticipant");
        }
    }
}