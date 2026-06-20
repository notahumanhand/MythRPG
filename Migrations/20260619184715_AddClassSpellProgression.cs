using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MythRPG.Migrations
{
    /// <inheritdoc />
    public partial class AddClassSpellProgression : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpellsPerLevel",
                table: "CharacterClasses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartingSpells",
                table: "CharacterClasses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpellsPerLevel",
                table: "CharacterClasses");

            migrationBuilder.DropColumn(
                name: "StartingSpells",
                table: "CharacterClasses");
        }
    }
}
