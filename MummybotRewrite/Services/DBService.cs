using Discord;
using Mummybot.Database.Models;
using System.Collections.Generic;
using Mummybot.Attributes;
using LiteDB;

namespace Mummybot.Services
{
    [Service(typeof(DBService))]
    public class DBService
    {
        [Inject]
       public LogService Logs { get; set; }

        readonly string db = "";


        public DBService()
        {
           db = new DatabaseDetails().LoadDetials().GetRuntimeDB();
        }
        public void Hooklog()
        {
            using (var database = new LiteDatabase(db))
            {
                database.Log.Logging +=(strng)=> Logs.LogInformation(strng,Enums.LogSource.Database);                
            }
        }

        public Guild GetGuildUncached(IGuild guild)
            => GetGuildUncached(guild.Id);
        public Guild GetGuildUncached(ulong guildid)
        {
            using (var database = new LiteDatabase(db))
            {
                return database.GetCollection<Guild>("Guilds").FindOne(g => g.GuildID == guildid);
            }
        }
        public IEnumerable<Guild> GetAllGuilds()
        {
            using (var database = new LiteDatabase(db))
            {
                return database.GetCollection<Guild>("Guilds").FindAll();
            }
        }

        public bool UpsertGuild(Guild guildsettings)
        {
            using (var database = new LiteDatabase(db))
            {
                var inorup=  database.GetCollection<Guild>("Guilds").Upsert(guildsettings);
                Logs.LogInformation($"{(inorup ? "inserted ": "updated ")} {guildsettings.GuildID}",Enums.LogSource.Database);

                return inorup;
            }
        }     
    }
}
