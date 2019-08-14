using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mummybot.Migrations.GuildStoreMigrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    GuildID = table.Column<decimal>(nullable: false),
                    AutoQuotes = table.Column<bool>(nullable: false, defaultValue: false),
                    StarboardChannelId = table.Column<decimal>(nullable: false),
                    StarboardEmote = table.Column<string>(nullable: true),
                    bdaychannelid = table.Column<decimal>(nullable: false),
                    UsesStarBoard = table.Column<bool>(nullable: false),
                    UsesReminders = table.Column<bool>(nullable: false),
                    UsesTags = table.Column<bool>(nullable: false),
                    UsesBirthdays = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.GuildID);
                });

            migrationBuilder.CreateTable(
                name: "Birthday",
                columns: table => new
                {
                    Id = table.Column<decimal>(nullable: false),
                    GuildID = table.Column<decimal>(nullable: false),
                    BDay = table.Column<DateTimeOffset>(nullable: false),
                    UserId = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Birthday", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Birthday_Guilds_GuildID",
                        column: x => x.GuildID,
                        principalTable: "Guilds",
                        principalColumn: "GuildID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prefixes",
                columns: table => new
                {
                    Id = table.Column<decimal>(nullable: false),
                    guildID = table.Column<decimal>(nullable: false),
                    Prefix = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prefixes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prefixes_Guilds_guildID",
                        column: x => x.guildID,
                        principalTable: "Guilds",
                        principalColumn: "GuildID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reminder",
                columns: table => new
                {
                    Id = table.Column<decimal>(nullable: false),
                    GuildID = table.Column<decimal>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    SetAtUTC = table.Column<DateTimeOffset>(nullable: false),
                    ExpiresAtUTC = table.Column<DateTimeOffset>(nullable: false),
                    UserID = table.Column<decimal>(nullable: false),
                    ChannelID = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reminder_Guilds_GuildID",
                        column: x => x.GuildID,
                        principalTable: "Guilds",
                        principalColumn: "GuildID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Star",
                columns: table => new
                {
                    Id = table.Column<decimal>(nullable: false),
                    GuildID = table.Column<decimal>(nullable: false),
                    MessageId = table.Column<decimal>(nullable: false),
                    StarboardMessageId = table.Column<decimal>(nullable: false),
                    Stars = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Star", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Star_Guilds_GuildID",
                        column: x => x.GuildID,
                        principalTable: "Guilds",
                        principalColumn: "GuildID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<decimal>(nullable: false),
                    UserId = table.Column<decimal>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Key = table.Column<string>(nullable: true),
                    GuildID = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_Guilds_GuildID",
                        column: x => x.GuildID,
                        principalTable: "Guilds",
                        principalColumn: "GuildID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Birthday_GuildID",
                table: "Birthday",
                column: "GuildID");

            migrationBuilder.CreateIndex(
                name: "IX_Prefixes_guildID",
                table: "Prefixes",
                column: "guildID");

            migrationBuilder.CreateIndex(
                name: "IX_Reminder_GuildID",
                table: "Reminder",
                column: "GuildID");

            migrationBuilder.CreateIndex(
                name: "IX_Star_GuildID",
                table: "Star",
                column: "GuildID");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_GuildID",
                table: "Tag",
                column: "GuildID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Birthday");

            migrationBuilder.DropTable(
                name: "Prefixes");

            migrationBuilder.DropTable(
                name: "Reminder");

            migrationBuilder.DropTable(
                name: "Star");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Guilds");
        }
    }
}
