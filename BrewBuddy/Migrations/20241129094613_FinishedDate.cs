using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrewBuddy.Migrations
{
    /// <inheritdoc />
    public partial class FinishedDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FinishedDateAndTime",
                table: "Assignments",
                newName: "DateAndTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateAndTime",
                table: "Assignments",
                newName: "FinishedDateAndTime");
        }
    }
}
