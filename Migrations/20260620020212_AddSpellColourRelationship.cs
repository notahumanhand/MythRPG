using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MythRPG.Migrations
{
    /// <inheritdoc />
    public partial class AddSpellColourRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Colour",
                table: "Spells",
                newName: "LegacyColour");

            migrationBuilder.AddColumn<int>(
                name: "ColourSpellColourId",
                table: "Spells",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Spells_ColourSpellColourId",
                table: "Spells",
                column: "ColourSpellColourId");

            migrationBuilder.AddForeignKey(
                name: "FK_Spells_SpellColours_ColourSpellColourId",
                table: "Spells",
                column: "ColourSpellColourId",
                principalTable: "SpellColours",
                principalColumn: "SpellColourId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spells_SpellColours_ColourSpellColourId",
                table: "Spells");

            migrationBuilder.DropIndex(
                name: "IX_Spells_ColourSpellColourId",
                table: "Spells");

            migrationBuilder.DropColumn(
                name: "ColourSpellColourId",
                table: "Spells");

            migrationBuilder.RenameColumn(
                name: "LegacyColour",
                table: "Spells",
                newName: "Colour");
        }
    }
}
