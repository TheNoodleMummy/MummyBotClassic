using Discord;
using Mummybot.Database.Models;
using Mummybot.Services;
using Qmmands;
using System;
using System.Threading.Tasks;

namespace Mummybot.Commands
{
    public abstract class MummyBase : ModuleBase<MummyContext>
    {

       

        public DBService Database { get; set; }
        public GuildService GuildService { get; set; }
        public Guild GuildConfig { get; private set; }
        public MessagesService Messages { get; set; }
        public LogService Logs { get; set; }
        public IServiceProvider Services { get; set; }

        public Task<IUserMessage> ReplyAsync(string message = null, bool isTTS = false, Embed embed = null, RequestOptions options = null)
        => Messages.SendMessageAsync(Context, message, isTTS, embed);

        protected override async Task BeforeExecutedAsync(Command command)
        {
            GuildConfig = await GuildService.GetGuildAsync(Context.Guild);

            await base.BeforeExecutedAsync(command);
        }
        protected override async Task AfterExecutedAsync(Command command)
        {
            await GuildService.UpdateGuildAsync(GuildConfig);
            await base.AfterExecutedAsync(command);
        }
    }
}
