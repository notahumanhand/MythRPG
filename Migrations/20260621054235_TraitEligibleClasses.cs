using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MythRPG.Migrations
{
    /// <inheritdoc />
    public partial class TraitEligibleClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterClassTrait");

            migrationBuilder.CreateTable(
                name: "CharacterClassGrantedTraits",
                columns: table => new
                {
                    CharacterClassId = table.Column<int>(type: "int", nullable: false),
                    GrantedTraitsTraitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterClassGrantedTraits", x => new { x.CharacterClassId, x.GrantedTraitsTraitId });
                    table.ForeignKey(
                        name: "FK_CharacterClassGrantedTraits_CharacterClasses_CharacterClassId",
                        column: x => x.CharacterClassId,
                        principalTable: "CharacterClasses",
                        principalColumn: "CharacterClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CharacterClassGrantedTraits_Traits_GrantedTraitsTraitId",
                        column: x => x.GrantedTraitsTraitId,
                        principalTable: "Traits",
                        principalColumn: "TraitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TraitEligibleClasses",
                columns: table => new
                {
                    EligibleClassesCharacterClassId = table.Column<int>(type: "int", nullable: false),
                    TraitId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraitEligibleClasses", x => new { x.EligibleClassesCharacterClassId, x.TraitId });
                    table.ForeignKey(
                        name: "FK_TraitEligibleClasses_CharacterClasses_EligibleClassesCharacterClassId",
                        column: x => x.EligibleClassesCharacterClassId,
                        principalTable: "CharacterClasses",
                        principalColumn: "CharacterClassId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TraitEligibleClasses_Traits_TraitId",
                        column: x => x.TraitId,
                        principalTable: "Traits",
                        principalColumn: "TraitId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterClassGrantedTraits_GrantedTraitsTraitId",
                table: "CharacterClassGrantedTraits",
                column: "GrantedTraitsTraitId");

            migrationBuilder.CreateIndex(
                name: "IX_TraitEligibleClasses_TraitId",
                table: "TraitEligibleClasses",
                column: "TraitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterClassGrantedTraits");

            migrationBuilder.DropTable(
                name: "TraitEligibleClasses");

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

            migrationBuilder.CreateIndex(
                name: "IX_CharacterClassTrait_GrantedTraitsTraitId",
                table: "CharacterClassTrait",
                column: "GrantedTraitsTraitId");
        }
    }
}
