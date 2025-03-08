using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mummybot.Migrations.GuildStoreMigrations
{
    public partial class Moar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    GuildID = table.Column<decimal>(nullable: false),
                    HangManRoleId = table.Column<decimal>(nullable: false),
                    HangManChannelID = table.Column<decimal>(nullable: false),
                    UsesHangman = table.Column<bool>(nullable: false),
                    AutoQuotes = table.Column<bool>(nullable: false, defaultValue: false),
                    StarboardChannelId = table.Column<decimal>(nullable: false),
                    StarboardEmote = table.Column<string>(nullable: true),
                    BdayChannelId = table.Column<decimal>(nullable: false),
                    UsesStarBoard = table.Column<bool>(nullable: false, defaultValue: false),
                    UsesReminders = table.Column<bool>(nullable: false, defaultValue: false),
                    UsesTags = table.Column<bool>(nullable: false, defaultValue: false),
                    UsesBirthdays = table.Column<bool>(nullable: false, defaultValue: false),
                    UsesMusic = table.Column<bool>(nullable: false, defaultValue: false),
                    UsesTrolls = table.Column<bool>(nullable: false, defaultValue: false),
                    AllowOffensiveCommands = table.Column<bool>(nullable: false),
                    Allow18PlusCommands = table.Column<bool>(nullable: false, defaultValue: false),
                    Volume = table.Column<int>(nullable: false, defaultValue: 20),
                    DefualtVolume = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.GuildID);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    word = table.Column<string>(nullable: true),
                    Issue = table.Column<string>(nullable: true),
                    used = table.Column<int>(nullable: false),
                    Reported = table.Column<bool>(nullable: false),
                    ReportedBy = table.Column<decimal>(nullable: false),
                    ReportedOn = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Birthday",
                columns: table => new
                {
                    Id = table.Column<decimal>(nullable: false),
                    GuildID = table.Column<decimal>(nullable: false),
                    BDay = table.Column<DateTimeOffset>(nullable: false),
                    NextBdayUTC = table.Column<DateTimeOffset>(nullable: false),
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
                name: "PlayListWhiteList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    GuildID = table.Column<decimal>(nullable: false),
                    UserId = table.Column<decimal>(nullable: false),
                    WhiteListedBy = table.Column<decimal>(nullable: false),
                    WhiteListedOn = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayListWhiteList", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayListWhiteList_Guilds_GuildID",
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

            migrationBuilder.CreateTable(
                name: "VoiceMutedUser",
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
                    table.PrimaryKey("PK_VoiceMutedUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoiceMutedUser_Guilds_GuildID",
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
                name: "IX_PlayListWhiteList_GuildID",
                table: "PlayListWhiteList",
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

            migrationBuilder.CreateIndex(
                name: "IX_VoiceDeafUser_GuildID",
                table: "VoiceDeafUser",
                column: "GuildID");

            migrationBuilder.CreateIndex(
                name: "IX_VoiceMutedUser_GuildID",
                table: "VoiceMutedUser",
                column: "GuildID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Birthday");

            migrationBuilder.DropTable(
                name: "PlayListWhiteList");

            migrationBuilder.DropTable(
                name: "Prefixes");

            migrationBuilder.DropTable(
                name: "Reminder");

            migrationBuilder.DropTable(
                name: "Star");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "VoiceDeafUser");

            migrationBuilder.DropTable(
                name: "VoiceMutedUser");

            migrationBuilder.DropTable(
                name: "Words");

            migrationBuilder.DropTable(
                name: "Guilds");
        }
    }
}
