using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Mummybot.Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Database
{
    public class GuildStore : DbContext
    {
        public DbSet<Guild> Guilds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer($"Data Source=thenoodlemummy.tk;Initial Catalog=MummybotRedo;User ID=MummyBot;Password=verymuchsecured;");

        public async Task<Guild> GetOrCreateGuildAsync(IGuild guild)
        {
            return await Guilds.FindAsync(guild.Id)?? await CreateGuildAsync(guild);
        }

        private async Task<Guild> CreateGuildAsync(IGuild guild)
        {
            var newguild = new Guild()
            {
                AutoQuotes = true,
                Prefixes = new List<string>() { "mummy", "!" },
                GuildID = guild.Id
            };
            await Guilds.AddAsync(newguild);

            await SaveChangesAsync();

            return await Guilds.FindAsync(guild.Id);
        }
    }
}
