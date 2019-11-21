using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mummybot.Migrations.GuildStoreMigrations
{
    public partial class extendingdbwordtoholdmoreinfoonreports : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ReportedBy",
                table: "Words",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReportedOn",
                table: "Words",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AlterColumn<decimal>(
                name: "Id",
                table: "PlayListWhiteList",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(20,0)")
                .OldAnnotation("SqlServer:Identity", "1, 1");

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
                name: "ReportedBy",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "ReportedOn",
                table: "Words");

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
