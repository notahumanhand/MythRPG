using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MythRPG.Migrations
{
    /// <inheritdoc />
    public partial class IncompatibilityHandling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IncompatibilityGroups",
                table: "Traits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncompatibilityGroups",
                table: "Traits");
        }
    }
}
