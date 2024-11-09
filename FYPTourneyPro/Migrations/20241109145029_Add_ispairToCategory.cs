using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FYPTourneyPro.Migrations
{
    /// <inheritdoc />
    public partial class Add_ispairToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isPair",
                table: "Category",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isPair",
                table: "Category");
        }
    }
}
