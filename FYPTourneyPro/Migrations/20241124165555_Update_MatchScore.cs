using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYPTourneyPro.Migrations
{
    /// <inheritdoc />
    public partial class Update_MatchScore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Score",
                table: "MatchScore",
                newName: "Set3Score");

            migrationBuilder.RenameColumn(
                name: "NumOfSets",
                table: "MatchScore",
                newName: "Set2Score");

            migrationBuilder.AddColumn<int>(
                name: "Set1Score",
                table: "MatchScore",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Set1Score",
                table: "MatchScore");

            migrationBuilder.RenameColumn(
                name: "Set3Score",
                table: "MatchScore",
                newName: "Score");

            migrationBuilder.RenameColumn(
                name: "Set2Score",
                table: "MatchScore",
                newName: "NumOfSets");
        }
    }
}
