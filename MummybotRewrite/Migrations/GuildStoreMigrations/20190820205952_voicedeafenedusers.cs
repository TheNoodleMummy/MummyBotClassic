using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mummybot.Migrations.GuildStoreMigrations
{
    public partial class voicedeafenedusers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VoiceDeafUser",
                columns: table => new
                {
                    Id = table.Column<decimal>(nullable: false),
                    GuildID = table.Column<decimal>(nullable: false),
                    SetAtUTC = table.Column<DateTimeOffset>(nullable: false),
                    ExpiresAtUTC = table.Column<DateTimeOffset>(nullable: false),
                    UserID = table.Column<decimal>(nullable: false),
                    ChannelID = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoiceDeafUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoiceDeafUser_Guilds_GuildID",
                        column: x => x.GuildID,
                        principalTable: "Guilds",
                        principalColumn: "GuildID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VoiceDeafUser_GuildID",
                table: "VoiceDeafUser",
                column: "GuildID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoiceDeafUser");
        }
    }
}
