using Discord;
using Mummybot.Attributes;
using Mummybot.Enums;
using Qmmands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [Name("Admin Module"), Description("Contains commands for admins to easy do admin related things")]
    public class AdminModule : MummyBase
    {
        [Command("config")]
        public async Task Showconfig()
        {
            var emb = new EmbedBuilder();
            emb.WithAuthor(a => { a.Name = Context.Guild.Name; a.IconUrl = Context.Guild.IconUrl; });
            emb.WithDescription(GuildConfig.GetConfig());

            await ReplyAsync(embed: emb.Build());


        }

        [Command("purge", "clean", "cleanup", "prune")]
        [Description("Cleans the bot's messages")]
        [RequireBotPermission(ChannelPermission.ManageMessages)]
        [RequireUserPermission(ChannelPermission.ManageMessages)]
        public async Task Clean(
        [Description("The optional number of messages to delete; defaults to 10")] int count = 10,
        [Description("The type of messages to delete - Self, Bot, or All")] DeleteType deleteType = DeleteType.Self,
        [Description("The strategy to delete messages - BulkDelete or Manual")] DeleteStrategy deleteStrategy = DeleteStrategy.Bulk)
        {
            int index = 0;
            var deleteMessages = new List<IMessage>(count);
            var messages = Context.Channel.GetMessagesAsync();
            await messages.ForEachAsync(async m =>
            {
                IEnumerable<IMessage> delete = null;
                if (deleteType == DeleteType.Self)
                    delete = m.Where(msg => msg.Author.Id == Context.Client.CurrentUser.Id);
                else if (deleteType == DeleteType.Bot)
                    delete = m.Where(msg => msg.Author.IsBot);
                else if (deleteType == DeleteType.All)
                    delete = m;

                foreach (var msg in delete.OrderByDescending(msg => msg.Timestamp))
                {
                    if (index >= count) { await EndClean(deleteMessages, deleteStrategy); return; }
                    deleteMessages.Add(msg);
                    index++;
                }
            });
        }


        [Description("Kicks the mentioned user"),
         Command("kick", "getlost"),
            RequireUserPermission(GuildPermission.KickMembers)]
        public Task Kick([Description("user to be kicked")]IUser user, [Description("Reason for kicking the user"), Remainder]string reason = null)
        => (user as IGuildUser).KickAsync(reason);


        [Description("Bans the Mentioned user"),
         Command("ban", "getlostforever"),
            RequireUserPermission(GuildPermission.BanMembers)]
        public Task Ban([Description("user to be banned")]IUser user, [Description("Reason for banning the user"), Remainder]string reason = null)
        => Context.Guild.AddBanAsync(user, 7, reason);

        [Description("Bans the Mentioned user"),
         Command("banid", "getlostforever"),
            RequireUserPermission(GuildPermission.BanMembers)]
        public Task BanID([Description("user to be banned")]ulong id, [Description("Reason for banning the user"), Remainder]string reason = null)
        => Context.Guild.AddBanAsync(id, 0, reason);




        internal async Task EndClean(IEnumerable<IMessage> messages, DeleteStrategy strategy)
        {
            if (strategy == DeleteStrategy.Bulk)
                await (Context.Channel as ITextChannel).DeleteMessagesAsync(messages);
            else if (strategy == DeleteStrategy.Manual)
            {
                foreach (var msg in messages.Cast<IUserMessage>())
                {
                    await msg.DeleteAsync();
                }
            }
        }
    }
}