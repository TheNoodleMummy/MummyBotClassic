using Discord;
using Microsoft.EntityFrameworkCore;
using Mummybot.Database.Entities;
using Mummybot.Services;
using System.Collections.Generic;
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
            });
            modelBuilder.Entity<Prefixes>(prefix=>
            {
                prefix.HasKey(x => x.Id);
            });
        }

        public async Task<Guild> GetOrCreateGuildAsync(IGuild guild)
        {
            return await Guilds.FindAsync(guild.Id) ?? await CreateGuildAsync(guild);
        }

        private async Task<Guild> CreateGuildAsync(IGuild guild)
        {
            var newguild = new Guild()
            {
                AutoQuotes = false,
                GuildID = guild.Id
            };
            newguild.Prefixes.Add(new Prefixes() { Prefix = "!" });
            await Guilds.AddAsync(newguild);

            await SaveChangesAsync();

            return await Guilds.FindAsync(guild.Id);
        }
    }
}
