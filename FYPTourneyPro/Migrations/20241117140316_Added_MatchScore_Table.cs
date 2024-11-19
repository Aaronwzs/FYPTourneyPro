using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYPTourneyPro.Migrations
{
    /// <inheritdoc />
    public partial class Added_MatchScore_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchScore",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MatchParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumOfSets = table.Column<int>(type: "integer", nullable: false),
                    Score = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchScore", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchScore");
        }
    }
}
