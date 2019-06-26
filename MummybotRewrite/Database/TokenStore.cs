using Microsoft.EntityFrameworkCore;
using Mummybot.Database.Entities;
using Mummybot.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Database
{
    public class TokenStore :DbContext
    {
        public DbSet<Token> Tokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
       => optionsBuilder.UseSqlServer(ConfigService.GetTokenDB());

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Token>(token =>
            {
                token.HasKey(x => x.BotName);
            });
        }
    }
}
