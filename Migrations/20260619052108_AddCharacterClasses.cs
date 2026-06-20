using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MythRPG.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "CharacterClasses",
                columns: table => new
                {
                    CharacterClassId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryBonus = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterClasses", x => x.CharacterClassId);
                });

            migrationBuilder.CreateTable(
                name: "SpellColours",
                columns: table => new
                {
                    SpellColourId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpellColours", x => x.SpellColourId);
                });

            migrationBuilder.CreateTable(
                name: "CharacterClassTrait",
                columns: table => new
                {
                    CharacterClassId = table.Column<int>(type: "int", nullable: false),
                    GrantedTraitsTraitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterClassTrait", x => new { x.CharacterClassId, x.GrantedTraitsTraitId });
                    table.ForeignKey(
                        name: "FK_CharacterClassTrait_CharacterClasses_CharacterClassId",
                        column: x => x.CharacterClassId,
                        principalTable: "CharacterClasses",
                        principalColumn: "CharacterClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterClassTrait_Traits_GrantedTraitsTraitId",
                        column: x => x.GrantedTraitsTraitId,
                        principalTable: "Traits",
                        principalColumn: "TraitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CharacterClassSpellColour",
                columns: table => new
                {
                    CharacterClassId = table.Column<int>(type: "int", nullable: false),
                    SpellColoursSpellColourId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterClassSpellColour", x => new { x.CharacterClassId, x.SpellColoursSpellColourId });
                    table.ForeignKey(
                        name: "FK_CharacterClassSpellColour_CharacterClasses_CharacterClassId",
                        column: x => x.CharacterClassId,
                        principalTable: "CharacterClasses",
                        principalColumn: "CharacterClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterClassSpellColour_SpellColours_SpellColoursSpellColourId",
                        column: x => x.SpellColoursSpellColourId,
                        principalTable: "SpellColours",
                        principalColumn: "SpellColourId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SpellColours",
                columns: new[] { "SpellColourId", "Name" },
                values: new object[,]
                {
                    { 1, "Black" },
                    { 2, "White" },
                    { 3, "Red" },
                    { 4, "Orange" },
                    { 5, "Gold" },
                    { 6, "Green" },
                    { 7, "Blue" },
                    { 8, "Purple" },
                    { 9, "Brown" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterClassSpellColour_SpellColoursSpellColourId",
                table: "CharacterClassSpellColour",
                column: "SpellColoursSpellColourId");

            migrationBuilder.CreateIndex(
                name: "IX_CharacterClassTrait_GrantedTraitsTraitId",
                table: "CharacterClassTrait",
                column: "GrantedTraitsTraitId");

            migrationBuilder.CreateIndex(
                name: "IX_SpellColours_Name",
                table: "SpellColours",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterClassSpellColour");

            migrationBuilder.DropTable(
                name: "CharacterClassTrait");

            migrationBuilder.DropTable(
                name: "SpellColours");

            migrationBuilder.DropTable(
                name: "CharacterClasses");
        }
    }
}
