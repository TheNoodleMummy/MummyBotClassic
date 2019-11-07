using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mummybot.Migrations.GuildStoreMigrations
{
    public partial class playlistwhitelist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlayListWhiteList",
                columns: table => new
                {
                    Id = table.Column<decimal>(nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_PlayListWhiteList_GuildID",
                table: "PlayListWhiteList",
                column: "GuildID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayListWhiteList");
        }
    }
}
