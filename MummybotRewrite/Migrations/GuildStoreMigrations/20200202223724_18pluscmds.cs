using Microsoft.EntityFrameworkCore.Migrations;

namespace Mummybot.Migrations.GuildStoreMigrations
{
    public partial class _18pluscmds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<bool>(
                name: "Allow18PlusCommands",
                table: "Guilds",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Allow18PlusCommands",
                table: "Guilds");

            migrationBuilder.AlterColumn<decimal>(
                name: "Id",
                table: "PlayListWhiteList",
                type: "decimal(20,0)",
                nullable: false,
                oldClrType: typeof(decimal))
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<decimal>(
                name: "GuildID",
                table: "Guilds",
                type: "decimal(20,0)",
                nullable: false,
                oldClrType: typeof(decimal))
                .Annotation("SqlServer:Identity", "1, 1");
        }
    }
}
