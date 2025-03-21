﻿using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Mummybot.Database.Entities;
using Mummybot.Services;
using Mummybot.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Mummybot.Database
{
    public class GuildStore : DbContext
    {
        [Obsolete("dont use this ya dummy but use the ctor with snowflake param")]
        public GuildStore() { }

        public GuildStore(SnowFlakeGeneratorService snowflakes)
        {
            SnowFlakeGeneratorService = snowflakes;
        }

        public DbSet<Guild> Guilds { get; set; }

        public DbSet<DBWord> Words { get; set; }

        private readonly SnowFlakeGeneratorService SnowFlakeGeneratorService;

        private readonly LogService _logservice = new LogService();

        public static SemaphoreSlim Slim = new SemaphoreSlim(1, 1);

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#if DEBUG
            => optionsBuilder.UseSqlServer(ConfigService.GetDebugDB());
#else
            => optionsBuilder.UseSqlServer(ConfigService.GetRuntimeDB());
#endif

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guild>(guild =>
            {
                guild.HasKey(g => g.GuildID);

                guild.Property(x => x.AutoQuotes)
                .HasDefaultValue(false);

                guild.Property(x => x.UsesBirthdays)
                .HasDefaultValue(false);

                guild.Property(x => x.UsesMusic)
                .HasDefaultValue(false);

                guild.Property(x => x.UsesReminders)
                .HasDefaultValue(false);

                guild.Property(x => x.UsesStarBoard)
                .HasDefaultValue(false);

                guild.Property(x => x.Volume)
                .HasDefaultValue(20);

                guild.Property(x => x.UsesTags)
                .HasDefaultValue(false);

                guild.Property(x => x.UsesTrolls)
                .HasDefaultValue(false);

                guild.Property(x => x.Allow18PlusCommands)
                .HasDefaultValue(false);

                guild.HasMany(x => x.Prefixes)
                .WithOne(y => y.Guild)
                .HasForeignKey(z => z.guildID);

                guild.HasMany(x => x.Stars)
                .WithOne(y => y.Guild)
                .HasForeignKey(z => z.GuildID);

                guild.HasMany(x => x.Birthdays)
                .WithOne(y => y.Guild)
                .HasForeignKey(z => z.GuildID);

                guild.HasMany(x => x.VoiceMutedUsers)
                .WithOne(y => y.Guild)
                .HasForeignKey(z => z.GuildID);

                guild.HasMany(x => x.VoiceDeafenedUsers)
                .WithOne(y => y.Guild)
                .HasForeignKey(z => z.GuildID);

                guild.HasMany(x => x.Reminders)
               .WithOne(y => y.Guild)
               .HasForeignKey(z => z.GuildID);

                guild.HasMany(x => x.Tags)
                .WithOne(y => y.Guild)
                .HasForeignKey(z => z.GuildID);

                guild.HasMany(x => x.PlayListWhiteLists)
                .WithOne(y => y.Guild)
                .HasForeignKey(z => z.GuildID);

            });
            modelBuilder.Entity<Prefixes>(prefix =>
            {
                prefix.HasKey(x => x.Id);
                prefix.Property(x => x.Id)
                .ValueGeneratedNever();
            });
            modelBuilder.Entity<Star>(star =>
            {
                star.HasKey(x => x.Id);
                star.Property(x => x.Id)
                .ValueGeneratedNever();
            });
            modelBuilder.Entity<Tag>(tag =>
            {
                tag.HasKey(x => x.Id);
                tag.Property(x => x.Id)
                .ValueGeneratedNever();
            });
            modelBuilder.Entity<Birthday>(birthday =>
            {
                birthday.HasKey(x => x.Id);
                birthday.Property(x => x.Id)
                .ValueGeneratedNever();
            });
            modelBuilder.Entity<PlayListWhiteList>(list =>
            {
                list.HasKey(x => x.Id);
                list.Property(x => x.Id)
                    .ValueGeneratedNever();
            });
            modelBuilder.Entity<VoiceMutedUser>(vmu =>
            {
                vmu.HasKey(x => x.Id);
                vmu.Property(x => x.Id)
                .ValueGeneratedNever();
            });
            modelBuilder.Entity<VoiceDeafUser>(vdu =>
            {
                vdu.HasKey(x => x.Id);
                vdu.Property(x => x.Id)
                .ValueGeneratedNever();
            });
            modelBuilder.Entity<Reminder>(reminder =>
            {
                reminder.HasKey(x => x.Id);
                reminder.Property(x => x.Id)
                .ValueGeneratedNever();
            });
            modelBuilder.Entity<DBWord>(word =>
            {
                word.HasKey(x => x.id);
                word.Property(x => x.id)
                .ValueGeneratedOnAdd();
            });
           
        }

        public Task<Guild> GetGuildForModule(IGuild guild)
       => GetGuildForModule(guild.Id);

        public Task<Guild> GetGuildForModule(ulong id)
        => Guilds.Include(g => g.Birthdays)
                .Include(g => g.Prefixes)
                .Include(g => g.Reminders)
                .Include(g => g.Stars)
                .Include(g => g.Tags)
                .Include(g => g.VoiceMutedUsers)
                .Include(g => g.VoiceDeafenedUsers)
                .Include(g => g.PlayListWhiteLists)
                .FirstOrDefaultAsync(x => x.GuildID == id);

        public async Task<Guild> GetOrCreateGuildAsync<TProp>(IGuild iguild, Expression<Func<Guild, TProp>> expression)
        {
            var guild = await Guilds.Include(expression).FirstOrDefaultAsync(g => g.GuildID == iguild.Id);
            if (guild is null)
            {
                _logservice.LogInformation($"guild: {iguild.Id} was not found creating new object", Enums.LogSource.GuildStore, iguild.Id);
                return await CreateGuildAsync(iguild.Id, expression);
            }
            return guild;
        }

        public async Task<Guild> GetOrCreateGuildAsync<TProp>(ulong guildid, Expression<Func<Guild, TProp>> expression)
        {
            var guild = await Guilds.Include(expression).FirstOrDefaultAsync(g => g.GuildID == guildid);
            if (guild is null)
            {
                _logservice.LogInformation($"guild: {guildid} was not found creating new object", Enums.LogSource.GuildStore, Guildid: guildid);
                return await CreateGuildAsync(guildid, expression);
            }
            return guild;
        }

        public async Task<Guild> GetOrCreateGuildAsync(ulong guildid)
        {
            var guild = await Guilds.FirstOrDefaultAsync(g => g.GuildID == guildid);
            if (guild is null)
            {
                _logservice.LogInformation($"guild: {guildid} was not found creating new object", Enums.LogSource.GuildStore, Guildid: guildid);
                return await CreateGuildAsync<ulong>(guildid: guildid, null);
            }
            return guild;
        }

        public async Task<Guild> GetOrCreateGuildAsync(IGuild iguild)
        {
            var guild = await Guilds.Include(g => g.Birthdays)
                .Include(g => g.Prefixes)
                .Include(g => g.Reminders)
                .Include(g => g.Stars)
                .Include(g => g.Tags)
                .Include(g => g.VoiceMutedUsers)
                .Include(g => g.VoiceDeafenedUsers)
                .Include(g => g.PlayListWhiteLists)
                .FirstOrDefaultAsync(g => g.GuildID == iguild.Id);
            if (guild is null)
            {
                _logservice.LogInformation($"guild: {iguild.Id} was not found creating new object", Enums.LogSource.GuildStore, iguild.Id);
                return await CreateGuildAsync<ulong>(guildid: iguild.Id, null);
            }
            return guild;
        }

        public Task<List<Guild>> GetAllGuildsAsync<TProp>(Expression<Func<Guild, TProp>> expression)
            => Guilds.Include(expression).ToListAsync();

        public Task<List<Guild>> GetAllGuildsAsync()
            => Guilds.ToListAsync();

        private async Task<Guild> CreateGuildAsync<TProp>(ulong guildid, Expression<Func<Guild, TProp>> expression)
        {
            await Slim.WaitAsync();
            var newguild = new Guild()
            {
                AutoQuotes = false,
                GuildID = guildid,
                Prefixes = new List<Prefixes>() { new Prefixes() { guildID = guildid, Prefix = "!", Id = SnowFlakeGeneratorService.NextLong() } }

            };
            await Guilds.AddAsync(newguild);

            if (expression is null)
            {
                return await Guilds.FindAsync(guildid);
            }
            Slim.Release();
            return await Guilds.Include(expression).FirstOrDefaultAsync(x => x.GuildID == guildid);
        }

       
    }
}
