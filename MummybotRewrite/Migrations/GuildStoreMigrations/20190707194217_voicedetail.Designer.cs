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
    [Migration("20190707194217_voicedetail")]
    partial class voicedetail
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mummybot.Database.Entities.Birthday", b =>
                {
                    b.Property<decimal>("Id")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.Property<DateTimeOffset>("BDay");

                    b.Property<decimal>("GuildID")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.Property<DateTimeOffset>("NextBdayUTC");

                    b.Property<decimal>("UserId")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.HasKey("Id");

                    b.HasIndex("GuildID");

                    b.ToTable("Birthday");
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Guild", b =>
                {
                    b.Property<decimal>("GuildID")
                        .ValueGeneratedOnAdd()
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.Property<bool>("AutoQuotes")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<decimal>("BdayChannelId")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.Property<int>("DefualtVolume");

                    b.Property<decimal>("StarboardChannelId")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.Property<string>("StarboardEmote");

                    b.Property<bool>("UsesBirthdays");

                    b.Property<bool>("UsesMusic");

                    b.Property<bool>("UsesReminders");

                    b.Property<bool>("UsesStarBoard");

                    b.Property<bool>("UsesTags");

                    b.Property<bool>("UsesTrolls");

                    b.Property<int>("Volume");

                    b.HasKey("GuildID");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Prefixes", b =>
                {
                    b.Property<decimal>("Id")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.Property<string>("Prefix");

                    b.Property<decimal>("guildID")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.HasKey("Id");

                    b.HasIndex("guildID");

                    b.ToTable("Prefixes");
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Reminder", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.Property<decimal>("ChannelID")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.Property<DateTimeOffset>("ExpiresAtUTC");

                    b.Property<decimal>("GuildID")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.Property<string>("Message");

                    b.Property<DateTimeOffset>("SetAtUTC");

                    b.Property<decimal>("UserID")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.HasKey("Id");

                    b.HasIndex("GuildID");

                    b.ToTable("Reminder");
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Star", b =>
                {
                    b.Property<decimal>("Id")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.Property<decimal>("GuildID")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.Property<decimal>("MessageId")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.Property<decimal>("StarboardMessageId")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.Property<int>("Stars");

                    b.HasKey("Id");

                    b.HasIndex("GuildID");

                    b.ToTable("Star");
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Tag", b =>
                {
                    b.Property<decimal>("Id")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.Property<decimal>("GuildID")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.Property<string>("Key");

                    b.Property<string>("Message");

                    b.Property<decimal>("UserId")
                        .HasConversion(new ValueConverter<decimal, decimal>(v => default(decimal), v => default(decimal), new ConverterMappingHints(precision: 20, scale: 0)));

                    b.HasKey("Id");

                    b.HasIndex("GuildID");

                    b.ToTable("Tag");
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Birthday", b =>
                {
                    b.HasOne("Mummybot.Database.Entities.Guild", "Guild")
                        .WithMany("Birthdays")
                        .HasForeignKey("GuildID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Prefixes", b =>
                {
                    b.HasOne("Mummybot.Database.Entities.Guild", "Guild")
                        .WithMany("Prefixes")
                        .HasForeignKey("guildID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Reminder", b =>
                {
                    b.HasOne("Mummybot.Database.Entities.Guild", "Guild")
                        .WithMany("Reminders")
                        .HasForeignKey("GuildID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Star", b =>
                {
                    b.HasOne("Mummybot.Database.Entities.Guild", "Guild")
                        .WithMany("Stars")
                        .HasForeignKey("GuildID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mummybot.Database.Entities.Tag", b =>
                {
                    b.HasOne("Mummybot.Database.Entities.Guild", "Guild")
                        .WithMany("Tags")
                        .HasForeignKey("GuildID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
