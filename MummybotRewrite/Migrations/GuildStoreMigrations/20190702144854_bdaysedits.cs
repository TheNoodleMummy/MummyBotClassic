using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mummybot.Migrations.GuildStoreMigrations
{
    public partial class bdaysedits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "BdayUTC",
                table: "Birthday",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BdayUTC",
                table: "Birthday");
        }
    }
}
