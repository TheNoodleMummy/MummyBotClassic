using Microsoft.EntityFrameworkCore.Migrations;

namespace Mummybot.Migrations.GuildStoreMigrations
{
    public partial class usesmusic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "bdaychannelid",
                table: "Guilds",
                newName: "BdayChannelId");

            migrationBuilder.AddColumn<bool>(
                name: "UsesMusic",
                table: "Guilds",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsesMusic",
                table: "Guilds");

            migrationBuilder.RenameColumn(
                name: "BdayChannelId",
                table: "Guilds",
                newName: "bdaychannelid");
        }
    }
}
