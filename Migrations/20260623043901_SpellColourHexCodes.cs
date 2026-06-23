using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MythRPG.Migrations
{
    /// <inheritdoc />
    public partial class SpellColourHexCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HexCode",
                table: "SpellColours",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "SpellColours",
                keyColumn: "SpellColourId",
                keyValue: 1,
                column: "HexCode",
                value: "#121214");

            migrationBuilder.UpdateData(
                table: "SpellColours",
                keyColumn: "SpellColourId",
                keyValue: 2,
                column: "HexCode",
                value: "#FFFFFF");

            migrationBuilder.UpdateData(
                table: "SpellColours",
                keyColumn: "SpellColourId",
                keyValue: 3,
                column: "HexCode",
                value: "#7D1616");

            migrationBuilder.UpdateData(
                table: "SpellColours",
                keyColumn: "SpellColourId",
                keyValue: 4,
                column: "HexCode",
                value: "#DF4200");

            migrationBuilder.UpdateData(
                table: "SpellColours",
                keyColumn: "SpellColourId",
                keyValue: 5,
                column: "HexCode",
                value: "#E08500");

            migrationBuilder.UpdateData(
                table: "SpellColours",
                keyColumn: "SpellColourId",
                keyValue: 6,
                column: "HexCode",
                value: "#2F4931");

            migrationBuilder.UpdateData(
                table: "SpellColours",
                keyColumn: "SpellColourId",
                keyValue: 7,
                column: "HexCode",
                value: "#3169D9");

            migrationBuilder.UpdateData(
                table: "SpellColours",
                keyColumn: "SpellColourId",
                keyValue: 8,
                column: "HexCode",
                value: "#971EA4");

            migrationBuilder.UpdateData(
                table: "SpellColours",
                keyColumn: "SpellColourId",
                keyValue: 9,
                column: "HexCode",
                value: "#4E3526");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HexCode",
                table: "SpellColours");
        }
    }
}
