

using Mummybot.Database.Entities;
using Mummybot.Extentions;
using Mummybot.Services;
using Qmmands;
using System;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [Group("tag")]
    public class TagModule : MummyBase
    {
        public SnowFlakeGeneratorService SnowFlakeGenerator { get; set; }
        [Command]
        public async Task TagAsync(string key)
        {
            var tag = GuildConfig.Tags.Find(t => t.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase));
            await ReplyAsync(tag.Message);
        }

        [Command("add")]
        public async Task AddTag(string key,[Remainder]string value)
        {
            var tag = new Tag() { Key = key, Message = value, Id = SnowFlakeGenerator.NextLong() };
            GuildConfig.Tags.Add(tag);
            await Context.Message.AddOkAsync();
        }

        [Group("edit")]
        public class EditTagModule : MummyBase
        {
            [Command("key")]
            public async Task EditKey(string key,string newkey)
            {
                var tag = GuildConfig.Tags.Find(t => t.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase));
                tag.Key = newkey;
                await Context.Message.AddOkAsync();
            }

            [Command("message")]
            public async Task EditMessage(string key,string newmessage)
            {
                var tag = GuildConfig.Tags.Find(t => t.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase));
                tag.Message = newmessage;
                await Context.Message.AddOkAsync();
            }
        }
    }
}
