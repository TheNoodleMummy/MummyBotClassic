using Microsoft.EntityFrameworkCore.Migrations;

namespace Mummybot.Migrations.GuildStoreMigrations
{
    public partial class voicedetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefualtVolume",
                table: "Guilds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "UsesTrolls",
                table: "Guilds",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Volume",
                table: "Guilds",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefualtVolume",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "UsesTrolls",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "Volume",
                table: "Guilds");
        }
    }
}
