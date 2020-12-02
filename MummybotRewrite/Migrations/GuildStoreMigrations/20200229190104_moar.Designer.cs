﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mummybot.Database;

namespace Mummybot.Migrations.GuildStoreMigrations
{
    [DbContext(typeof(GuildStore))]
    [Migration("20200229190104_moar")]
    partial class moar
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mummybot.Database.Entities.Birthday", b =>
                {
                    b.Property<decimal>("Id")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTimeOffset>("BDay")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal>("GuildID")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTimeOffset>("NextBdayUTC")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("GuildID");

                    b.ToTable("Birthday");
                });

            modelBuilder.Entity("Mummybot.Database.Entities.DBWord", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Issue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Reported")
                        .HasColumnType("bit");

                    b.Property<decimal>("ReportedBy")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTimeOffset>("ReportedOn")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("used")
                        .HasColumnType("int");

                    b.Property<string>("word")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("id");

                    b.ToTable("Words");
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Guild", b =>
                {
                    b.Property<decimal>("GuildID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("decimal(20,0)");

                    b.Property<bool>("Allow18PlusCommands")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("AllowOffensiveCommands")
                        .HasColumnType("bit");

                    b.Property<bool>("AutoQuotes")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<decimal>("BdayChannelId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<int>("DefualtVolume")
                        .HasColumnType("int");

                    b.Property<decimal>("HangManChannelID")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("HangManRoleId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("StarboardChannelId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("StarboardEmote")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("UsesBirthdays")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("UsesHangman")
                        .HasColumnType("bit");

                    b.Property<bool>("UsesMusic")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("UsesReminders")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("UsesStarBoard")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("UsesTags")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("UsesTrolls")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("Volume")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(20);

                    b.HasKey("GuildID");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("Mummybot.Database.Entities.PlayListWhiteList", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<decimal>("GuildID")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("WhiteListedBy")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTimeOffset>("WhiteListedOn")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("GuildID");

                    b.ToTable("PlayListWhiteList");
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Prefixes", b =>
                {
                    b.Property<decimal>("Id")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("Prefix")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("guildID")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("guildID");

                    b.ToTable("Prefixes");
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Reminder", b =>
                {
                    b.Property<decimal>("Id")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("ChannelID")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTimeOffset>("ExpiresAtUTC")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal>("GuildID")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("SetAtUTC")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal>("UserID")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("GuildID");

                    b.ToTable("Reminder");
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Star", b =>
                {
                    b.Property<decimal>("Id")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("GuildID")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("MessageId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("StarboardMessageId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<int>("Stars")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GuildID");

                    b.ToTable("Star");
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Tag", b =>
                {
                    b.Property<decimal>("Id")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("GuildID")
                        .HasColumnType("decimal(20,0)");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("UserId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("GuildID");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("Mummybot.Database.Entities.VoiceDeafUser", b =>
                {
                    b.Property<decimal>("Id")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("ChannelID")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTimeOffset>("ExpiresAtUTC")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal>("GuildID")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTimeOffset>("SetAtUTC")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal>("UserID")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("GuildID");

                    b.ToTable("VoiceDeafUser");
                });

            modelBuilder.Entity("Mummybot.Database.Entities.VoiceMutedUser", b =>
                {
                    b.Property<decimal>("Id")
                        .HasColumnType("decimal(20,0)");

                    b.Property<decimal>("ChannelID")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTimeOffset>("ExpiresAtUTC")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal>("GuildID")
                        .HasColumnType("decimal(20,0)");

                    b.Property<DateTimeOffset>("SetAtUTC")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal>("UserID")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("GuildID");

                    b.ToTable("VoiceMutedUser");
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Birthday", b =>
                {
                    b.HasOne("Mummybot.Database.Entities.Guild", "Guild")
                        .WithMany("Birthdays")
                        .HasForeignKey("GuildID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mummybot.Database.Entities.PlayListWhiteList", b =>
                {
                    b.HasOne("Mummybot.Database.Entities.Guild", "Guild")
                        .WithMany("PlayListWhiteLists")
                        .HasForeignKey("GuildID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Prefixes", b =>
                {
                    b.HasOne("Mummybot.Database.Entities.Guild", "Guild")
                        .WithMany("Prefixes")
                        .HasForeignKey("guildID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Reminder", b =>
                {
                    b.HasOne("Mummybot.Database.Entities.Guild", "Guild")
                        .WithMany("Reminders")
                        .HasForeignKey("GuildID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Star", b =>
                {
                    b.HasOne("Mummybot.Database.Entities.Guild", "Guild")
                        .WithMany("Stars")
                        .HasForeignKey("GuildID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Tag", b =>
                {
                    b.HasOne("Mummybot.Database.Entities.Guild", "Guild")
                        .WithMany("Tags")
                        .HasForeignKey("GuildID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mummybot.Database.Entities.VoiceDeafUser", b =>
                {
                    b.HasOne("Mummybot.Database.Entities.Guild", "Guild")
                        .WithMany("VoiceDeafenedUsers")
                        .HasForeignKey("GuildID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Mummybot.Database.Entities.VoiceMutedUser", b =>
                {
                    b.HasOne("Mummybot.Database.Entities.Guild", "Guild")
                        .WithMany("VoiceMutedUsers")
                        .HasForeignKey("GuildID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}