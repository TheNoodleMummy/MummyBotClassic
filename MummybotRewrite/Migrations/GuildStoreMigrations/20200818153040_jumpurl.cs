using Microsoft.EntityFrameworkCore.Migrations;

namespace Mummybot.Migrations.GuildStoreMigrations
{
    public partial class jumpurl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JumpUrl",
                table: "Reminder",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "GuildID",
                table: "Guilds",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JumpUrl",
                table: "Reminder");

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
