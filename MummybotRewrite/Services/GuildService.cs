using Discord;
using Mummybot.Attributes;
using Mummybot.Database.Models;
using Mummybot.interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Mummybot.Services
{
    [Service("Guild Service",typeof(GuildService))]
    public class GuildService : IRemoveableService
    {
        [Inject]
        public TimerService TimerService { get; set; }
        [Inject]
        public LogService Logs { get; set; }
        [Inject]
        public DBService DBService { get; set; }

        public ConcurrentDictionary<ulong, Guild> _GuildCache = new ConcurrentDictionary<ulong, Guild>();
        public string db = "";

        public GuildService()
        {
            db = new DatabaseDetails().LoadDetials().GetRuntimeDB();
        }

        public Task<Guild> GetGuildAsync(IGuild guild)
        => GetGuildAsync(guild.Id);
        public async Task<Guild> GetGuildAsync(ulong id)
        {
            if (_GuildCache.TryGetValue(id, out Guild guild))
            {                
                guild.When = DateTime.Now.AddDays(1);
                await TimerService.UpdateAsync(guild);
                return guild;
            }
            else
            {
                var guildtoreturn = new Guild(DBService.GetGuildUncached(id) ?? await NewGuildAsync(id), this)
                {
                    When = DateTime.Now.AddDays(1)
                };
                _GuildCache.TryAdd(id, guildtoreturn);
                await UpdateGuildAsync(guildtoreturn);
                Logs.LogDebug($"loaded {id} into cache", Enums.LogSource.GuildService);
                return guildtoreturn;
            }

        }

        public async Task UpdateGuildAsync(Guild guild)
        {
            DBService.UpsertGuild(guild);


            guild.When = DateTime.Now.AddDays(1);
            var gld = new Guild(guild, this);
            _GuildCache.TryAdd(guild.GuildID, gld);

            await TimerService.UpdateAsync(gld);

        }

        public async Task<Guild> NewGuildAsync(ulong id)
        {

            var newguild = new Guild(this)
            {
                GuildID = id
            };
            await UpdateGuildAsync(newguild);
            return newguild;
        }



        public async Task RemoveAsync(IRemoveable obj)
        {
            if ((obj is Guild guild))
                if (_GuildCache.TryRemove(guild.GuildID, out Guild _))
                {
                    Logs.LogInformation($"Succesfully removed {guild.GuildID} from cache", Enums.LogSource.GuildService);
                    DBService.UpsertGuild(guild);
                    await Task.CompletedTask;

                }
                else
                {
                    Logs.LogError($"Failed To Remove {guild.GuildID} From Cache, Retrying In 1 Hour", Enums.LogSource.GuildService);
                    guild.When = DateTime.Now.AddHours(1);
                    await UpdateGuildAsync(guild);
                    await TimerService.UpdateAsync(guild);
                }
        }
    }
}
