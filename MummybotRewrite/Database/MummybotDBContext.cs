//using Microsoft.EntityFrameworkCore;
//using System;
//using Mummybot.Database.Models;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Threading.Tasks;
//using Discord;
//using System.Linq;

//namespace Mummybot.Database
//{
//    public class MummyDBContext : DbContext, IDisposable
//    {
//        public MummyDBContext(DbContextOptions options) : base(options) { }
//        public MummyDBContext() { }

//        //public async Task<Guild> GetGuildAsync(IGuild guild)
//        //{
//        //    using (var database = new MummyDBContext())
//        //    {
//        //        var guildtoreturn = database.GuildConfigs.FirstOrDefault(g => g.GuildID == guild.Id) ?? await NewGuildAsync(guild);
//        //        return guildtoreturn;
//        //    }
//        //}

//        //public async Task UpdateGuildAsync(Guild guildsettings)
//        //{
//        //    using (var database = new MummyDBContext())
//        //    {
//        //        database.GuildConfigs.Update(guildsettings);
//        //        await database.SaveChangesAsync();
//        //    }
//        //}

//        //public async Task<Guild> NewGuildAsync(IGuild guild)
//        //{
//        //    using (var database = new MummyDBContext())
//        //    {
//        //        var newguild = new Guild
//        //        {
//        //            GuildID = guild.Id,
//        //            PrefixesAsStrings = "!"
//        //        };
//        //        database.GuildConfigs.Add(newguild);
//        //        await database.SaveChangesAsync();
//        //        return newguild;
//        //    }


//        //}

//        public DbSet<TokenData> Tokens { get;set; }
//        public DbSet<Guild> GuildConfigs { get; set; }

//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
//                optionsBuilder.UseSqlServer(DatabaseDetailsExtend.GetconectionString());
//            }
//        }

//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Guild>().Property(c => c.__GuildID).IsRequired();
//            modelBuilder.Entity<Guild>().HasMany(p => p.Stars).WithOne();
//            modelBuilder.Entity<Guild>().HasMany(p => p.Reminders).WithOne();
//            modelBuilder.Entity<Guild>().HasMany(p => p.Birthdays).WithOne();
//            modelBuilder.Entity<Guild>().HasMany(p => p.Tags).WithOne();

//            modelBuilder.Entity<TokenData>().Property(x => x.Name).IsRequired(); 
//        }

//        public override void Dispose()
//        {
//            this.SaveChanges();
//            base.Dispose();
//        }
//    }

//}