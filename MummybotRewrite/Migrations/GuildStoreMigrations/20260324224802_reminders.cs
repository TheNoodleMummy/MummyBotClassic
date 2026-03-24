using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mummybot.Migrations.GuildStoreMigrations
{
    /// <inheritdoc />
    public partial class reminders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OriginalMessageId",
                table: "Reminder",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalMessageId",
                table: "Reminder");
        }
    }
}
