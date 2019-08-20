using Discord;
using Microsoft.EntityFrameworkCore;
using Mummybot.Database.Entities;
using Mummybot.Services;
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

                guild.HasMany(x => x.Reminders)
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
            modelBuilder.Entity<VoiceMutedUser>(vmu =>
            {
                vmu.HasKey(x => x.Id);
                vmu.Property(x => x.Id)
                .ValueGeneratedNever();
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
                .FirstOrDefaultAsync(x => x.GuildID == id);

        public async Task<Guild> GetOrCreateGuildAsync<TProp>(IGuild iguild, Expression<Func<Guild, TProp>> expression)
        {
            await Slim.WaitAsync();
            var guild = await Guilds.Include(expression).FirstOrDefaultAsync(g => g.GuildID == iguild.Id);
            if (guild is null)
            {
                _logservice.LogInformation($"guild: {iguild.Id} was not found creating new object");
                return await CreateGuildAsync(iguild.Id, expression);
            }
            Slim.Release();
            return guild;
        }

        public async Task<Guild> GetOrCreateGuildAsync<TProp>(ulong guildid, Expression<Func<Guild, TProp>> expression)
        {
            await Slim.WaitAsync();
            var guild = await Guilds.Include(expression).FirstOrDefaultAsync(g => g.GuildID == guildid);
            if (guild is null)
            {
                _logservice.LogInformation($"guild: {guildid} was not found creating new object");
                return await CreateGuildAsync(guildid, expression);
            }
            Slim.Release();
            return guild;
        }

        public async Task<Guild> GetOrCreateGuildAsync(ulong guildid)
        {
            await Slim.WaitAsync();
            var guild = await Guilds.FirstOrDefaultAsync(g => g.GuildID == guildid);
            if (guild is null)
            {
                _logservice.LogInformation($"guild: {guildid} was not found creating new object");
                return await CreateGuildAsync<ulong>(guildid: guildid, null);
            }
            Slim.Release();
            return guild;
        }

        public async Task<Guild> GetOrCreateGuildAsync(IGuild iguild)
        {
            await Slim.WaitAsync();
            var guild = await Guilds.FirstOrDefaultAsync(g => g.GuildID == iguild.Id);
            if (guild is null)
            {
                _logservice.LogInformation($"guild: {iguild.Id} was not found creating new object");
                return await CreateGuildAsync<ulong>(guildid: iguild.Id, null);
            }
            Slim.Release();
            return guild;
        }

        public Task<List<Guild>> GetAllGuildsAsync<TProp>(Expression<Func<Guild, TProp>> expression)
            => Guilds.Include(expression).ToListAsync();

        public Task<List<Guild>> GetAllGuildsAsync()
            => Guilds.ToListAsync();

        private async Task<Guild> CreateGuildAsync<TProp>(ulong guildid, Expression<Func<Guild, TProp>> expression)
        {
            var newguild = new Guild()
            {
                AutoQuotes = false,
                GuildID = guildid,
                Prefixes = new List<Prefixes>() { new Prefixes() { guildID = guildid, Prefix = "!", Id = SnowFlakeGeneratorService.NextLong() } }

            };
            await Guilds.AddAsync(newguild);

            var i = await SaveChangesAsync();
            Console.WriteLine("guildstore " + i);
            if (expression is null)
            {
                return await Guilds.FindAsync(guildid);
            }
            return await Guilds.Include(expression).FirstOrDefaultAsync(x => x.GuildID == guildid);
        }
    }
}
