using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MythRPG.Migrations
{
    /// <inheritdoc />
    public partial class AddCharacterVisibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Characters",
                keyColumn: "CharacterId",
                keyValue: 1,
                column: "IsPublic",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Characters");
        }
    }
}
