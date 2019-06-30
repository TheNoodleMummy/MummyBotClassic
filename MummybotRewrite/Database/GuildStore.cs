using Discord;
using Microsoft.EntityFrameworkCore;
using Mummybot.Database.Entities;
using Mummybot.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mummybot.Database
{
    public class GuildStore : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer(ConfigService.GetRuntimeDB());

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
            });
            modelBuilder.Entity<Prefixes>(prefix=>
            {
                prefix.HasKey(x => x.Id);
                prefix.Property(x => x.Id).ValueGeneratedNever();
            });
            modelBuilder.Entity<Star>(star =>
            {
                star.HasKey(x => x.Id);
                star.Property(x => x.Id).ValueGeneratedNever();
            });
        }

        public async Task<Guild> GetOrCreateGuildAsync<TProp>(IGuild guild, Expression<Func<Guild, TProp>> expression)
        {
            return await Guilds.Include(expression).FirstOrDefaultAsync(g => g.GuildID == guild.Id)
                ?? await CreateGuildAsync(guild.Id, expression);
        }

        public async Task<Guild> GetOrCreateGuildAsync<TProp>(ulong guildid, Expression<Func<Guild, TProp>> expression)
        {
            return await Guilds.Include(expression).FirstOrDefaultAsync(g => g.GuildID == guildid)
                ?? await CreateGuildAsync(guildid, expression);
        }

        public async Task<Guild> GetOrCreateGuildAsync(IGuild guild)
        {
            return await Guilds.FirstOrDefaultAsync(g => g.GuildID == guild.Id)
                ?? await CreateGuildAsync<ulong>(guild.Id, null);
        }

        public Task<List<Guild>> GetAlllGuildsAsync<TProp>(Expression<Func<Guild, TProp>> expression)
            => Guilds.Include(expression).ToListAsync();

        public Task<List<Guild>> GetAllGuildsAsync()
            => Guilds.ToListAsync();

        private async Task<Guild> CreateGuildAsync<TProp>(ulong guildid, Expression<Func<Guild, TProp>> expression)
        {
            var newguild = new Guild()
            {
                AutoQuotes = false,
                GuildID = guildid
            };
            newguild.Prefixes.Add(new Prefixes() { Prefix = "!" });
            await Guilds.AddAsync(newguild);

            await SaveChangesAsync();
            if (expression is null)
            {
                await Guilds.FindAsync(guildid);
            }
            return await Guilds.Include(expression).FirstOrDefaultAsync(x=>x.GuildID == guildid);
        }
    }
}
