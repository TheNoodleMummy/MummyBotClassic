using Microsoft.EntityFrameworkCore.Migrations;

namespace Mummybot.Migrations.GuildStoreMigrations
{
    public partial class bdayedits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BdayUTC",
                table: "Birthday",
                newName: "NextBdayUTC");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NextBdayUTC",
                table: "Birthday",
                newName: "BdayUTC");
        }
    }
}
